using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Susep.SISRH.Application.Options;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using SUSEP.Framework.Utils.Abstractions;
using SUSEP.Framework.Utils.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class ResponderSolitacaoPactoTrabalhoAtividadeCommandHandler : IRequestHandler<ResponderSolitacaoPactoTrabalhoAtividadeCommand, IActionResult>
    {
        private IItemCatalogoRepository ItemCatalogoRepository { get; }
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }

        private IEstruturaOrganizacionalQuery EstruturaOrganizacionalQuery { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IPessoaQuery PessoaQuery { get; }
        private IEmailHelper EmailHelper { get; }
        private IUnidadeQuery UnidadeQuery { get; }
        private IPactoTrabalhoQuery PactoTrabalhoQuery { get; }
        private IOptions<PadroesOptions> Configuration { get; }
        private IOptions<EmailOptions> EmailConfiguration { get; }

        public ResponderSolitacaoPactoTrabalhoAtividadeCommandHandler(
            IItemCatalogoRepository itemCatalogoRepository,
            IPactoTrabalhoRepository planoTrabalhoRepository,
            IEstruturaOrganizacionalQuery estruturaOrganizacionalQuery,
            IPactoTrabalhoQuery pactoTrabalhoQuery,
            IPessoaQuery pessoaQuery,
            IEmailHelper emailHelper,
            IUnidadeQuery unidadeQuery,
            IUnitOfWork unitOfWork,
            IOptions<PadroesOptions> configuration,
            IOptions<EmailOptions> emailConfiguration)
        {
            ItemCatalogoRepository = itemCatalogoRepository;
            PactoTrabalhoRepository = planoTrabalhoRepository;
            EstruturaOrganizacionalQuery = estruturaOrganizacionalQuery;
            PactoTrabalhoQuery = pactoTrabalhoQuery;
            PessoaQuery = pessoaQuery;
            UnitOfWork = unitOfWork;
            PessoaQuery = pessoaQuery;
            EmailHelper = emailHelper;
            UnidadeQuery = unidadeQuery;
            Configuration = configuration;
            EmailConfiguration = emailConfiguration;
        }

        public async Task<IActionResult> Handle(ResponderSolitacaoPactoTrabalhoAtividadeCommand request, CancellationToken cancellationToken)
        {
            var result = new ApplicationResult<PactoTrabalhoViewModel>(request);

            try
            {
                if (!await validarPermissoes(request))
                {
                    result.SetHttpStatusToForbidden("O usuário logado não possui permissão suficientes para executar a ação.");
                    return result;
                }

                //Monta os dados do pacto de trabalho
                var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

                //Obtém os dados da solicitação
                var solicitacao = pacto.Solicitacoes.Where(s => s.PactoTrabalhoSolicitacaoId == request.PactoTrabalhoSolicitacaoId).FirstOrDefault();

                var dataFim = pacto.DataFim;

                Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate.ItemCatalogo itemCatalogo = null;
                if (request.AjustarPrazo)
                {
                    //Obtém os dias não úteis da pessoa
                    var dias = diasAumentoPrazo(pacto, solicitacao);

                    if (dias == null)
                    {
                        result.SetHttpStatusToBadRequest("Não foi possível recuperar os dias de aumento de prazo.");
                        return result;
                    }

                    dataFim = pacto.DataFim.AddDays(Convert.ToDouble(Decimal.Round(dias.Value)));                    
                }

                dynamic dadosSolicitacao = JsonConvert.DeserializeObject(solicitacao.DadosSolicitacao);
                switch (solicitacao.TipoSolicitacaoId)
                {
                    case (int)SISRH.Domain.Enums.TipoSolicitacaoPactoTrabalhoEnum.AlterarPrazo:                        
                        dataFim = (DateTime)dadosSolicitacao.dataFim;
                        break;

                    case (int)SISRH.Domain.Enums.TipoSolicitacaoPactoTrabalhoEnum.NovaAtividade:
                        Guid itemCatalogoId = dadosSolicitacao.itemCatalogoId;
                        itemCatalogo = await ItemCatalogoRepository.ObterAsync(itemCatalogoId);
                        break;
                }                

                var diasNaoUteis = await PessoaQuery.ObterDiasNaoUteisAsync(pacto.PessoaId, pacto.DataInicio, dataFim);
                pacto.DiasNaoUteis = diasNaoUteis.Result.ToList();

                //Responde a solicitação
                pacto.ResponderSolicitacao(request.PactoTrabalhoSolicitacaoId, request.UsuarioLogadoId.ToString(), request.Aprovado, request.AjustarPrazo, request.Descricao, itemCatalogo);

                //Altera o pacto de trabalho no banco de dados
                PactoTrabalhoRepository.Atualizar(pacto);
                UnitOfWork.Commit(false);

                //Envia os emails aos envolvidos
                await EnviarEmail(request.PactoTrabalhoId, pacto.PessoaId, pacto.UnidadeId);

                var dadosPacto = await PactoTrabalhoQuery.ObterPorChaveAsync(request.PactoTrabalhoId);
                result.Result = dadosPacto.Result;
                result.SetHttpStatusToOk("Pacto de trabalho alterado com sucesso.");
            }
            catch (SISRH.Domain.Exceptions.SISRHDomainException ex)
            {
                result.Validations = new List<string>() { ex.Message };
                result.Result = null;
                result.SetHttpStatusToBadRequest();
            }
            return result;
        }

        #region EnviarEmail

        private async Task EnviarEmail(Guid pactoTrabalhoId, Int64 pessoaId, Int64 unidadeId)
        {
            try
            {
                if (Configuration.Value.Notificacoes == null ||
                    Configuration.Value.Notificacoes.EnviarEmail)
                {
                    //Obtem os destinatários dos emails
                    var destinatarios = new List<string>();

                    var servidor = await PessoaQuery.ObterPorChaveAsync(pessoaId);
                    destinatarios.Add(servidor.Result.Email);

                    var unidade = await UnidadeQuery.ObterPessoasAsync(unidadeId);
                    var chefes = unidade.Result.Where(u => u.UnidadeId == unidadeId && u.TipoFuncaoId.HasValue);
                    foreach (var chefe in chefes)
                        if (!string.IsNullOrEmpty(chefe.Email))
                            destinatarios.Add(chefe.Email);

                    //Envia os emails
                    EnviarEmail(pactoTrabalhoId, Configuration.Value.Notificacoes.EmailPactoSolicitacaoAnalisada, destinatarios.ToArray());
                }
            }
            catch { }
        }

        private void EnviarEmail(Guid pactoTrabalhoId, Email opcaoEmail, string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var enderecoAcesso = Configuration.Value.EnderecoPublicacaoFront.TrimEnd('/') + "/programagestao/pactotrabalho/detalhar/" + pactoTrabalhoId.ToString();

                var mensagem = new StringBuilder();
                mensagem.Append(opcaoEmail.Mensagem)
                    .AppendLine().AppendLine().Append("<a href =\"").Append(enderecoAcesso).Append("\">Clique aqui</a> para acessar o plano no sistema.").AppendLine().AppendLine()
                    .AppendLine("Caso o link não funcione, copie o endereço abaixo e abra no navegador da sua preferência:")
                    .AppendLine(enderecoAcesso);

                EmailHelper.Send(
                    EmailConfiguration.Value.EmailRemetente,
                    EmailConfiguration.Value.NomeRemetente,
                    destinatarios,
                    opcaoEmail.Assunto,
                    mensagem.ToString(),
                    true);
            }
        }

        #endregion


        private Decimal? diasAumentoPrazo(Domain.AggregatesModel.PactoTrabalhoAggregate.PactoTrabalho pacto, PactoTrabalhoSolicitacao solicitacao)
        {
            dynamic dadosSolicitacao = JsonConvert.DeserializeObject(solicitacao.DadosSolicitacao);

            if (solicitacao != null)
            {
                switch (solicitacao.TipoSolicitacaoId)
                {
                    case ((int)TipoSolicitacaoPactoTrabalhoEnum.JustificarEstouroPrazo):
                        Guid? pactoTrabalhoAtividadeId = dadosSolicitacao.pactoTrabalhoAtividadeId;
                        if (pactoTrabalhoAtividadeId != null)
                        {
                            var atividade = pacto.Atividades.Where(a => a.PactoTrabalhoAtividadeId == pactoTrabalhoAtividadeId).FirstOrDefault();
                            if (atividade != null)
                            {
                                return atividade.DiferencaPrevistoParaRealizadoEmDias * 5;
                            }
                        }
                        break;

                    case ((int)TipoSolicitacaoPactoTrabalhoEnum.NovaAtividade):
                        decimal tempoPrevistoPorItem = dadosSolicitacao.tempoPrevistoPorItem;
                        return Decimal.Divide(tempoPrevistoPorItem, pacto.Pessoa.CargaHoraria) * 5;
                }
            }

            return null;
        }

        private async Task<bool> validarPermissoes(ResponderSolitacaoPactoTrabalhoAtividadeCommand request)
        {
            if (request.AjustarPrazo)
            {
                //Recupera os perfis do usuario logado
                var pessoaPerfil = await EstruturaOrganizacionalQuery.ObterPerfilPessoaAsync(request);
                var perfis = pessoaPerfil.Result.Perfis;

                bool podeAjustarOPrazo = perfis.Where(p =>
                    p.Perfil == (int)PerfilUsuarioEnum.Administrador ||
                    p.Perfil == (int)PerfilUsuarioEnum.Gestor ||
                    p.Perfil == (int)PerfilUsuarioEnum.ChefeUnidade).Count() > 0;

                return podeAjustarOPrazo;
            }

            return true;
        }

    }

}
