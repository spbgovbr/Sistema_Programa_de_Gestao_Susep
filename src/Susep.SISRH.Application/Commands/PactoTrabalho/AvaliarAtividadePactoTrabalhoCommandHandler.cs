using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
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
    public class AvaliarAtividadePactoTrabalhoCommandHandler : IRequestHandler<AvaliarAtividadePactoTrabalhoCommand, IActionResult>
    {

        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IPessoaQuery PessoaQuery { get; }
        private IEmailHelper EmailHelper { get; }
        private IUnidadeQuery UnidadeQuery { get; }

        public AvaliarAtividadePactoTrabalhoCommandHandler(
            IPactoTrabalhoRepository planoTrabalhoRepository,
            IUnitOfWork unitOfWork,
            IPessoaQuery pessoaQuery,
            IEmailHelper emailHelper,
            IUnidadeQuery unidadeQuery)
        {
            PactoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
            PessoaQuery = pessoaQuery;
            EmailHelper = emailHelper;
            UnidadeQuery = unidadeQuery;
        }

        public async Task<IActionResult> Handle(AvaliarAtividadePactoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            //Monta os dados do pacto de trabalho
            var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

            //Altera a atividade
            pacto.AvaliarAtividade(request.PactoTrabalhoAtividadeId, request.Nota, request.Justificativa);

            //Altera o pacto de trabalho no banco de dados
            PactoTrabalhoRepository.Atualizar(pacto);
            UnitOfWork.Commit(false);

            //Envia os emails aos envolvidos
            await EnviarEmail(pacto.PessoaId, pacto.UnidadeId);

            result.Result = true;
            result.SetHttpStatusToOk("Atividade avaliada com sucesso.");
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
                         .AppendLine("Uma atividade em um plano de trabalho em que você está envolvido foi avaliada.").AppendLine("")
                         .AppendLine("Acompanhe o andamento por meio do sistema.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Avaliação de atividade",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
            }
        }

        #endregion


    }
}
