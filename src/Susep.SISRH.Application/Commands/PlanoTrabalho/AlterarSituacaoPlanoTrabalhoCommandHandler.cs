using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Susep.SISRH.Application.Options;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
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

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class AlterarSituacaoPlanoTrabalhoCommandHandler : IRequestHandler<AlterarSituacaoPlanoTrabalhoCommand, IActionResult>
    {
        private IPlanoTrabalhoRepository PlanoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IEmailHelper EmailHelper { get; }
        private IUnidadeQuery UnidadeQuery { get; }
        private IOptions<PadroesOptions> Configuration { get; }
        private IOptions<EmailOptions> EmailConfiguration { get; }

        public AlterarSituacaoPlanoTrabalhoCommandHandler(
            IPlanoTrabalhoRepository planoTrabalhoRepository,
            IUnitOfWork unitOfWork,
            IEmailHelper emailHelper,
            IUnidadeQuery unidadeQuery,
            IOptions<PadroesOptions> configuration,
            IOptions<EmailOptions> emailConfiguration)
        {
            PlanoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
            EmailHelper = emailHelper;
            UnidadeQuery = unidadeQuery;
            Configuration = configuration;
            EmailConfiguration = emailConfiguration;
        }

        public async Task<IActionResult> Handle(AlterarSituacaoPlanoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            //Monta o objeto com os dados do catalogo
            var item = await PlanoTrabalhoRepository.ObterAsync(request.PlanoTrabalhoId);

            try
            {
                //Altera a situação do plano
                if (request.Deserto)
                    item.AlterarSituacao((int)SituacaoPlanoTrabalhoEnum.Concluido, request.UsuarioLogadoId.ToString(), request.Observacoes, true);
                else if (request.SituacaoId == (int)SituacaoPlanoTrabalhoEnum.EmExecucao)
                    item.ConcluirHabilitacao(request.UsuarioLogadoId.ToString(), request.Observacoes, request.Aprovados);
                else
                    item.AlterarSituacao(request.SituacaoId, request.UsuarioLogadoId.ToString(), request.Observacoes);

                //Altera o item de catalogo no banco de dados
                PlanoTrabalhoRepository.Atualizar(item);
                UnitOfWork.Commit(false);

                //Notifica os envolvidos
                await EnviarEmails(item);

                result.Result = true;
                result.SetHttpStatusToOk("Plano de trabalho alterado com sucesso.");
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

        private async Task EnviarEmails(Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate.PlanoTrabalho item)
        {
            try
            {

                if (Configuration.Value.Notificacoes == null ||
                    Configuration.Value.Notificacoes.EnviarEmail)
                {

                    switch (item.SituacaoId)
                    {
                        case (int)SituacaoPlanoTrabalhoEnum.EnviadoAprovacao:
                            //Obtém o chefe, o CG e o diretor da unidade
                            var destsAprovacao = await ObterChefesUnidade(item.Unidade.SiglaCompleta);
                            EnviarEmailPlanoEnviadoAprovacao(item.PlanoTrabalhoId, Configuration.Value.Notificacoes.EmailPlanoParaAprovacao, destsAprovacao);
                            break;

                        case (int)SituacaoPlanoTrabalhoEnum.Aprovado:
                            //Obtém o chefe, o CG e o diretor da unidade
                            var destsAprovado = await ObterChefesUnidade(item.Unidade.SiglaCompleta);
                            EnviarEmailPlanoAprovado(item.PlanoTrabalhoId, Configuration.Value.Notificacoes.EmailPlanoAprovado, destsAprovado);
                            break;


                        case (int)SituacaoPlanoTrabalhoEnum.Rejeitado:
                            //Obtém o chefe, o CG e o diretor da unidade
                            var destsRejeitado = await ObterChefesUnidade(item.Unidade.SiglaCompleta);
                            EnviarEmailPlanoRejeitado(item.PlanoTrabalhoId, Configuration.Value.Notificacoes.EmailPlanoRejeitado, destsRejeitado);
                            break;

                        case (int)SituacaoPlanoTrabalhoEnum.Habilitacao:

                            //Obtém as pessoas da unidade
                            var pessoasUnidade = await UnidadeQuery.ObterPessoasAsync(item.UnidadeId);
                            var pessoasEnviarEmail = pessoasUnidade.Result;
                            //Se o tipo de notificação não incluir as pessoas da subunidade, 
                            //  Deve adicionar apenas as pessoas que estão diretamente lotadas na unidade OU 
                            //  que tem função em unidades inferiores
                            if (Configuration.Value.Notificacoes == null ||
                                Configuration.Value.Notificacoes.AberturaFaseHabilitacao != "IncluirSubunidades")
                            {
                                pessoasEnviarEmail = pessoasEnviarEmail.Where(p => p.UnidadeId == item.UnidadeId || (p.Chefe.HasValue && p.Chefe.Value)).ToList();
                            }
                            EnviarEmailHabilitacao(item.PlanoTrabalhoId, Configuration.Value.Notificacoes.EmailPlanoEmHabilitacao, pessoasEnviarEmail.Where(p => !String.IsNullOrEmpty(p.Email)).Select(p => p.Email).ToArray());
                            break;


                        //Conclusão da fase de habilitação
                        case (int)SituacaoPlanoTrabalhoEnum.ProntoParaExecucao:

                            //Obtém as pessoas que tiveram a candidatura aprovada
                            var aprovados = from a in item.Atividades
                                            from c in a.Candidatos
                                            where c.SituacaoId == (int)SituacaoCandidaturaPlanoTrabalhoEnum.Aprovada
                                            select c.Pessoa;

                            //Obtém as pessoas que não foram aprovadas
                            var rejeitados = from a in item.Atividades
                                             from c in a.Candidatos
                                             where c.SituacaoId != (int)SituacaoCandidaturaPlanoTrabalhoEnum.Aprovada
                                             select c.Pessoa;
                            rejeitados = rejeitados.Where(r => !aprovados.Any(a => a.PessoaId == r.PessoaId));

                            //Envia email aos aprovados e aos rejeitados
                            EnviarEmailCandidaturaAprovada(item.PlanoTrabalhoId, Configuration.Value.Notificacoes.EmailPlanoCandidaturaAprovada, aprovados.Where(p => !String.IsNullOrEmpty(p.Email)).Select(p => p.Email).ToArray());
                            EnviarEmailCandidaturaRejeitada(item.PlanoTrabalhoId, Configuration.Value.Notificacoes.EmailPlanoCandidaturaRejeitada, rejeitados.Where(p => !String.IsNullOrEmpty(p.Email)).Select(p => p.Email).ToArray());

                            break;
                    }
                }
            }
            catch { }

        }

        private async Task<string[]> ObterChefesUnidade(string siglaCompletaUnidade)
        {
            var chefes = await UnidadeQuery.ObterChefesAsync(siglaCompletaUnidade);
            return chefes.Result.Where(p => !String.IsNullOrEmpty(p.Email)).Select(p => p.Email).ToArray();
        }

        private void EnviarEmailPlanoEnviadoAprovacao(Guid planoTrabalhoId, Email opcaoEmail, string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = FormatarMensagem(planoTrabalhoId, opcaoEmail);

                EmailHelper.Send(
                    EmailConfiguration.Value.EmailRemetente,
                    EmailConfiguration.Value.NomeRemetente,
                    destinatarios,
                    opcaoEmail.Assunto,
                    mensagem.ToString(),
                    true);
            }
        }

        private void EnviarEmailPlanoAprovado(Guid planoTrabalhoId, Email opcaoEmail, string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = FormatarMensagem(planoTrabalhoId, opcaoEmail);

                EmailHelper.Send(
                    EmailConfiguration.Value.EmailRemetente,
                    EmailConfiguration.Value.NomeRemetente,
                    destinatarios,
                    opcaoEmail.Assunto,
                    mensagem.ToString(),
                    true);
            }
        }

        private void EnviarEmailPlanoRejeitado(Guid planoTrabalhoId, Email opcaoEmail, string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = FormatarMensagem(planoTrabalhoId, opcaoEmail);

                EmailHelper.Send(
                    EmailConfiguration.Value.EmailRemetente,
                    EmailConfiguration.Value.NomeRemetente,
                    destinatarios,
                    opcaoEmail.Assunto,
                    mensagem.ToString(),
                    true);
            }
        }

        private void EnviarEmailHabilitacao(Guid planoTrabalhoId, Email opcaoEmail, string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = FormatarMensagem(planoTrabalhoId, opcaoEmail);

                EmailHelper.Send(
                    EmailConfiguration.Value.EmailRemetente,
                    EmailConfiguration.Value.NomeRemetente,
                    destinatarios,
                    opcaoEmail.Assunto,
                    mensagem.ToString(),
                    true);
            }
        }

        private void EnviarEmailCandidaturaAprovada(Guid planoTrabalhoId, Email opcaoEmail, string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = FormatarMensagem(planoTrabalhoId, opcaoEmail);

                EmailHelper.Send(
                    EmailConfiguration.Value.EmailRemetente,
                    EmailConfiguration.Value.NomeRemetente,
                    destinatarios,
                    opcaoEmail.Assunto,
                    mensagem.ToString(),
                    true);
            }
        }

        private void EnviarEmailCandidaturaRejeitada(Guid planoTrabalhoId, Email opcaoEmail, string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = FormatarMensagem(planoTrabalhoId, opcaoEmail);

                EmailHelper.Send(
                    EmailConfiguration.Value.EmailRemetente,
                    EmailConfiguration.Value.NomeRemetente,
                    destinatarios,
                    opcaoEmail.Assunto,
                    mensagem,
                    true);
            }
        }

        private string FormatarMensagem(Guid planoTrabalhoId, Email opcaoEmail)
        {
            var enderecoAcesso = Configuration.Value.EnderecoPublicacaoFront.TrimEnd('/') + "/programagestao/detalhar/" + planoTrabalhoId.ToString();

            var mensagem = new StringBuilder();
            mensagem.Append(opcaoEmail.Mensagem)
                .AppendLine().AppendLine().AppendLine().Append("<a href =\"").Append(enderecoAcesso).Append("\">Clique aqui</a> para acessar o programa de gestão no sistema.").AppendLine().AppendLine()
                .AppendLine("Caso o link não funcione, copie o endereço abaixo e abra no navegador da sua preferência:")
                .AppendLine(enderecoAcesso);
            return mensagem.ToString();
        }

        #endregion
    }
}
