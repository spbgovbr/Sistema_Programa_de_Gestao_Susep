using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Susep.SISRH.Application.Queries.Abstractions;
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
    public class JustificarEstouroPrazoAtividadeCommandHandler : IRequestHandler<JustificarEstouroPrazoAtividadeCommand, IActionResult>
    {
        private IItemCatalogoRepository ItemCatalogoRepository { get; }
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }
        private IPessoaQuery PessoaQuery { get; }
        private IEmailHelper EmailHelper { get; }
        private IUnidadeQuery UnidadeQuery { get; }

        public JustificarEstouroPrazoAtividadeCommandHandler(
            IItemCatalogoRepository itemCatalogoRepository,
            IPactoTrabalhoRepository planoTrabalhoRepository,
            IUnitOfWork unitOfWork,
            IPessoaQuery pessoaQuery,
            IEmailHelper emailHelper,
            IUnidadeQuery unidadeQuery)
        {
            ItemCatalogoRepository = itemCatalogoRepository;
            PactoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
            PessoaQuery = pessoaQuery;
            EmailHelper = emailHelper;
            UnidadeQuery = unidadeQuery;
        }

        public async Task<IActionResult> Handle(JustificarEstouroPrazoAtividadeCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);            

            //Monta os dados do pacto de trabalho
            var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

            //Serializa a requisição
            var dados = JsonConvert.SerializeObject(request);

            //Cria a solicitação
            pacto.AdicionarSolicitacao(TipoSolicitacaoPactoTrabalhoEnum.JustificarEstouroPrazo, request.UsuarioLogadoId.ToString(), dados, request.Justificativa);

            //Adiciona a solicitação no banco
            PactoTrabalhoRepository.Atualizar(pacto);
            UnitOfWork.Commit(false);

            //Envia os emails aos envolvidos
            await EnviarEmail(pacto.PessoaId, pacto.UnidadeId);

            result.Result = true;
            result.SetHttpStatusToOk("Justificativa de estouro de prazo adicionada com sucesso.");
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
                         .AppendLine("Uma justificativa de estouro de prazo na execução de uma atividade em um plano de trabalho em que você está envolvido foi solicitada.").AppendLine("")
                         .AppendLine("Acompanhe o andamento por meio do sistema.");

                EmailHelper.Send(
                    "naoresponda@susep.gov.br",
                    "[Programa de gestão] Solicitação de justificativa de estouro de prazo registrada",
                    destinatarios,
                    "Sistema de Gestão de Pessoas - Programa de gestão",
                    mensagem.ToString(), true);
            }
        }

        #endregion

    }
}
