using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Susep.SISRH.Application.Options;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
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
    public class AlterarSituacaoPactoTrabalhoCommandHandler : IRequestHandler<AlterarSituacaoPactoTrabalhoCommand, IActionResult>
    {
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IPessoaQuery PessoaQuery { get; }
        private IEmailHelper EmailHelper { get; }
        private IUnidadeQuery UnidadeQuery { get; }
        private IOptions<PadroesOptions> Configuration { get; }
        private IOptions<EmailOptions> EmailConfiguration { get; }

        public AlterarSituacaoPactoTrabalhoCommandHandler(            
            IPactoTrabalhoRepository pactoTrabalhoRepository,
            IUnitOfWork unitOfWork,
            IPessoaQuery pessoaQuery,
            IEmailHelper emailHelper,
            IUnidadeQuery unidadeQuery,
            IOptions<PadroesOptions> configuration,
            IOptions<EmailOptions> emailConfiguration)
        {
            PactoTrabalhoRepository = pactoTrabalhoRepository;
            UnitOfWork = unitOfWork;
            PessoaQuery = pessoaQuery;
            EmailHelper = emailHelper;
            UnidadeQuery = unidadeQuery;
            Configuration = configuration;
            EmailConfiguration = emailConfiguration;
        }

        public async Task<IActionResult> Handle(AlterarSituacaoPactoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            try
            {
                //Monta o objeto com os dados do catalogo
                var item = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

                //Obtém os dias não úteis da pessoa
                var diasNaoUteis = await PessoaQuery.ObterDiasNaoUteisAsync(item.PessoaId, item.DataInicio, item.DataFim);
                item.DiasNaoUteis = diasNaoUteis.Result.ToList();

                //Alterar a situação do plano de trabalho
                item.AlterarSituacao(request.SituacaoId, request.UsuarioLogadoId.ToString(), request.Observacoes);

                //Altera o plano de trabalho no banco de dados
                PactoTrabalhoRepository.Atualizar(item);
                UnitOfWork.Commit(false);

                //Envia os emails aos envolvidos
                await EnviarEmail(request.PactoTrabalhoId, item.PessoaId, item.UnidadeId);

                result.Result = true;
                result.SetHttpStatusToOk("Pacto de trabalho alterado com sucesso.");
            }
            catch (SISRH.Domain.Exceptions.SISRHDomainException ex)
            {
                result.Validations = new List<string>() { ex.Message };
                result.Result = false;
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
                    EnviarEmail(pactoTrabalhoId, Configuration.Value.Notificacoes.EmailPactoSituacaoAlterada, destinatarios.ToArray());
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

    }
}
