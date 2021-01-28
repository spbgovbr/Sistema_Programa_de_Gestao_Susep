using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Queries.Concrete;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
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

namespace Susep.SISRH.Application.Commands.PlanoTrabalho
{
    public class AlterarSituacaoPlanoTrabalhoCommandHandler : IRequestHandler<AlterarSituacaoPlanoTrabalhoCommand, IActionResult>
    {
        private IPlanoTrabalhoRepository PlanoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IEmailHelper EmailHelper { get; }
        private IUnidadeQuery UnidadeQuery { get; }

        public AlterarSituacaoPlanoTrabalhoCommandHandler(            
            IPlanoTrabalhoRepository planoTrabalhoRepository, 
            IUnitOfWork unitOfWork,
            IEmailHelper emailHelper,
            IUnidadeQuery unidadeQuery
        )
        {
            PlanoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
            EmailHelper = emailHelper;
            UnidadeQuery = unidadeQuery;
        }

        public async Task<IActionResult> Handle(AlterarSituacaoPlanoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);            

            //Monta o objeto com os dados do catalogo
            var item = await PlanoTrabalhoRepository.ObterAsync(request.PlanoTrabalhoId);

            try
            {
                //Altera a situação do plano
                if (request.SituacaoId == (int)SituacaoPlanoTrabalhoEnum.EmExecucao)
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
                switch (item.SituacaoId)
                {
                    case (int)SituacaoPlanoTrabalhoEnum.EnviadoAprovacao:
                        //Obtém o chefe, o CG e o diretor da unidade
                        var destsAprovacao = await ObterChefesUnidade(item.Unidade.SiglaCompleta);
                        EnviarEmailPlanoEnviadoAprovacao(destsAprovacao);
                        break;

                    case (int)SituacaoPlanoTrabalhoEnum.Aprovado:
                        //Obtém o chefe, o CG e o diretor da unidade
                        var destsAprovado = await ObterChefesUnidade(item.Unidade.SiglaCompleta);
                        EnviarEmailPlanoAprovado(destsAprovado);
                        break;


                    case (int)SituacaoPlanoTrabalhoEnum.Rejeitado:
                        //Obtém o chefe, o CG e o diretor da unidade
                        var destsRejeitado = await ObterChefesUnidade(item.Unidade.SiglaCompleta);
                        EnviarEmailPlanoRejeitado(destsRejeitado);
                        break;

                    case (int)SituacaoPlanoTrabalhoEnum.Habilitacao:

                        //Obtém as pessoas da unidade
                        var pessoasUnidade = await UnidadeQuery.ObterPessoasAsync(item.UnidadeId);
                        EnviarEmailHabilitacao(pessoasUnidade.Result.Where(p => !String.IsNullOrEmpty(p.Email)).Select(p => p.Email).ToArray());
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
                        EnviarEmailCandidaturaAprovada(aprovados.Where(p => !String.IsNullOrEmpty(p.Email)).Select(p => p.Email).ToArray());
                        EnviarEmailCandidaturaRejeitada(rejeitados.Where(p => !String.IsNullOrEmpty(p.Email)).Select(p => p.Email).ToArray());

                        break;
                }
            }
            catch { }

        }

        private async Task<string[]> ObterChefesUnidade(string siglaCompletaUnidade)
        {
            var chefes = await UnidadeQuery.ObterChefesAsync(siglaCompletaUnidade);
            return chefes.Result.Where(p => !String.IsNullOrEmpty(p.Email)).Select(p => p.Email).ToArray();
        }

        private void EnviarEmailPlanoEnviadoAprovacao(string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = new StringBuilder();

                mensagem.AppendLine($"Prezado(a), ").AppendLine("")
                        .AppendLine("Um programa de gestão em unidade sob sua gestão foi enviado para aprovação.").AppendLine("")
                        .AppendLine("Acompanhe o andamento por meio do sistema.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Programa de gestão enviado para aprovação",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
            }
        }

        private void EnviarEmailPlanoAprovado(string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = new StringBuilder();

                mensagem.AppendLine($"Prezado(a), ").AppendLine("")
                        .AppendLine("Um programa de gestão em unidade sob sua gestão foi aprovado.").AppendLine("")
                        .AppendLine("Acompanhe o andamento por meio do sistema.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Programa de gestão aprovado",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
            }
        }

        private void EnviarEmailPlanoRejeitado(string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = new StringBuilder();

                mensagem.AppendLine($"Prezado(a), ").AppendLine("")
                         .AppendLine("Um programa de gestão em unidade sob sua gestão foi rejeitado.").AppendLine("")
                         .AppendLine("Acompanhe o andamento por meio do sistema.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Programa de gestão rejeitado",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
            }
        }

        private void EnviarEmailHabilitacao(string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = new StringBuilder();

                mensagem.AppendLine($"Prezado(a), ").AppendLine("")
                            .AppendLine("Está aberta a fase de habilitação de um programa de gestão na sua unidade.").AppendLine("")
                            .AppendLine("Acesse o sistema e, se for do seu interesse, candidate-se a uma das vagas.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Habilitação iniciada",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
            }
        }

        private void EnviarEmailCandidaturaAprovada(string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = new StringBuilder();

                mensagem.AppendLine($"Prezado(a), ").AppendLine("")
                            .AppendLine("Sua candidatura para vaga no programa de gestão foi aprovada.").AppendLine("")
                            .AppendLine("Aguarde até que a execução do programa de gestão seja iniciada para que possa fazer o apontamento das suas atividades.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Candidatura aprovada",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
            }
        }

        private void EnviarEmailCandidaturaRejeitada(string[] destinatarios)
        {
            destinatarios = destinatarios.Where(d => !String.IsNullOrEmpty(d)).ToArray();
            if (destinatarios.Any())
            {
                var mensagem = new StringBuilder();

                mensagem.AppendLine($"Prezado(a), ").AppendLine("")
                            .AppendLine("Sua candidatura para vaga no programa de gestão não foi aprovada.").AppendLine("")
                            .AppendLine("Entre em contato com a sua chefia para entender os motivos.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Candidatura rejeitada",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
            }
        }

        #endregion
    }
}
