using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
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
    public class CandidatarPlanoTrabalhoAtividadeCommandHandler : IRequestHandler<CandidatarPlanoTrabalhoAtividadeCommand, IActionResult>
    {
        private IPlanoTrabalhoRepository PlanoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IEmailHelper EmailHelper { get; }
        private IPessoaQuery PessoaQuery { get; }

        public CandidatarPlanoTrabalhoAtividadeCommandHandler(            
            IPlanoTrabalhoRepository planoTrabalhoRepository, 
            IUnitOfWork unitOfWork,
            IEmailHelper emailHelper,
            IPessoaQuery pessoaQuery)
        {
            PlanoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
            EmailHelper = emailHelper;
            PessoaQuery = pessoaQuery;
        }

        public async Task<IActionResult> Handle(CandidatarPlanoTrabalhoAtividadeCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);            

            //Monta o objeto com os dados do catalogo
            var item = await PlanoTrabalhoRepository.ObterAsync(request.PlanoTrabalhoId);

            //Remove a atividade
            item.RegistrarCandidaturaAtividade(request.PlanoTrabalhoAtividadeId, request.UsuarioLogadoId);

            //Altera o item de catalogo no banco de dados
            PlanoTrabalhoRepository.Atualizar(item);
            UnitOfWork.Commit(false);

            //Envia email
            await EnviarEmail(request.UsuarioLogadoId);

            result.Result = true;
            result.SetHttpStatusToOk("Candidatura registrada com sucesso.");
            return result;
        }

        #region EnviarEmail

        private async Task EnviarEmail(Int64 pessoaId)
        {
            try
            {

                //Obtém os dados do usuário logado
                var dadosPessoa = await PessoaQuery.ObterPorChaveAsync(pessoaId);
                if (!String.IsNullOrEmpty(dadosPessoa.Result.Email))
                {
                    //Obtem os destinatários dos emails
                    var destinatarios = new List<string>();
                    destinatarios.Add(dadosPessoa.Result.Email);

                    //Envia os emails
                    EnviarEmail(destinatarios.ToArray());
                }
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
                        .AppendLine("Sua candidatura a uma vaga no programa de gestão foi registrada.").AppendLine("")
                        .AppendLine("Acompanhe o andamento por meio do sistema.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Candidatura no programa de gestão registrada",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
            }
        }

        #endregion

    }
}
