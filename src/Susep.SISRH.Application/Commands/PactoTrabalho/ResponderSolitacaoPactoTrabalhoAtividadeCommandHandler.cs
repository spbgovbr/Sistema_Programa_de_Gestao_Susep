using IdentityServer4.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Queries.Concrete;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using SUSEP.Framework.Utils.Abstractions;
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

        public ResponderSolitacaoPactoTrabalhoAtividadeCommandHandler(
            IItemCatalogoRepository itemCatalogoRepository,
            IPactoTrabalhoRepository planoTrabalhoRepository,
            IEstruturaOrganizacionalQuery estruturaOrganizacionalQuery,
            IPessoaQuery pessoaQuery,
            IEmailHelper emailHelper,
            IUnidadeQuery unidadeQuery,
            IUnitOfWork unitOfWork)
        {
            ItemCatalogoRepository = itemCatalogoRepository;
            PactoTrabalhoRepository = planoTrabalhoRepository;
            EstruturaOrganizacionalQuery = estruturaOrganizacionalQuery;
            PessoaQuery = pessoaQuery;
            UnitOfWork = unitOfWork;
            PessoaQuery = pessoaQuery;
            EmailHelper = emailHelper;
            UnidadeQuery = unidadeQuery;
        }

        public async Task<IActionResult> Handle(ResponderSolitacaoPactoTrabalhoAtividadeCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            if (!await validarPermissoes(request))
            {
                result.SetHttpStatusToForbidden("O usuário logado não possui permissão suficientes para executar a ação.");
                return result;
            }

            //Monta os dados do pacto de trabalho
            var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

            //Obtém os dados da solicitação
            var solicitacao = pacto.Solicitacoes.Where(s => s.PactoTrabalhoSolicitacaoId == request.PactoTrabalhoSolicitacaoId).FirstOrDefault();

            if (request.AjustarPrazo)
            {
                //Obtém os dias não úteis da pessoa
                var dias = diasAumentoPrazo(pacto, solicitacao);

                if (dias == null)
                {
                    result.SetHttpStatusToBadRequest("Não foi possível recuperar os dias de aumento de prazo.");
                    return result;
                }

                var diasNaoUteis = await PessoaQuery.ObterDiasNaoUteisAsync(pacto.PessoaId, pacto.DataInicio, pacto.DataFim.AddDays(Convert.ToDouble(Decimal.Round(dias.Value))));
                pacto.DiasNaoUteis = diasNaoUteis.Result.ToList();
            }
            else if (solicitacao.TipoSolicitacaoId == (int)SISRH.Domain.Enums.TipoSolicitacaoPactoTrabalhoEnum.AlterarPrazo)
            {
                dynamic dadosSolicitacao = JsonConvert.DeserializeObject(solicitacao.DadosSolicitacao);
                var diasNaoUteis = await PessoaQuery.ObterDiasNaoUteisAsync(pacto.PessoaId, pacto.DataFim, (DateTime)dadosSolicitacao.dataFim);
                pacto.DiasNaoUteis = diasNaoUteis.Result.ToList();
            }

            //Responde a solicitação
            pacto.ResponderSolicitacao(request.PactoTrabalhoSolicitacaoId, request.UsuarioLogadoId.ToString(), request.Aprovado, request.AjustarPrazo, request.Descricao);

            //Altera o pacto de trabalho no banco de dados
            PactoTrabalhoRepository.Atualizar(pacto);
            UnitOfWork.Commit(false);

            //Envia os emails aos envolvidos
            await EnviarEmail(pacto.PessoaId, pacto.UnidadeId);

            result.Result = true;
            result.SetHttpStatusToOk("Pacto de trabalho alterado com sucesso.");
            return result;
        }

        #region EnviarEmail

        private async Task EnviarEmail(Int64 pessoaId, Int64 unidadeId)
        {
            try
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
                EnviarEmail(destinatarios.ToArray());
            }
            catch { }
        }

        private void EnviarEmail(string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = new StringBuilder();

                mensagem.AppendLine($"Prezado(a), ").AppendLine("")
                        .AppendLine("Uma solicitação de alteração de um plano de trabalho em que você está envolvido foi analisada.").AppendLine("")
                        .AppendLine("Acompanhe o andamento por meio do sistema.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Solicitação analisada",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
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
                                return atividade.diferencaPrevistoParaRealizadoEmDias;
                            }
                        }
                        break;

                    case ((int)TipoSolicitacaoPactoTrabalhoEnum.NovaAtividade):
                        decimal tempoPrevistoPorItem = dadosSolicitacao.tempoPrevistoPorItem;
                        return Decimal.Divide(tempoPrevistoPorItem, pacto.Pessoa.CargaHoraria);
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
