using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class CadastrarPactoTrabalhoAtividadeCommandHandler : IRequestHandler<CadastrarPactoTrabalhoAtividadeCommand, IActionResult>
    {
        private IItemCatalogoRepository ItemCatalogoRepository { get; }
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IPessoaQuery PessoaQuery { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarPactoTrabalhoAtividadeCommandHandler(
            IItemCatalogoRepository itemCatalogoRepository,
            IPactoTrabalhoRepository planoTrabalhoRepository,
            IPessoaQuery pessoaQuery,
            IUnitOfWork unitOfWork)
        {
            ItemCatalogoRepository = itemCatalogoRepository;
            PactoTrabalhoRepository = planoTrabalhoRepository;
            PessoaQuery = pessoaQuery;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarPactoTrabalhoAtividadeCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            request.AssuntosId = request.AssuntosId != null ? request.AssuntosId : new List<Guid>();
            request.ObjetosId = request.ObjetosId != null ? request.ObjetosId : new List<Guid>();

            //Monta os dados do pacto de trabalho
            var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

            //Obtém os dias não úteis da pessoa
            var diasNaoUteis = await PessoaQuery.ObterDiasNaoUteisAsync(pacto.PessoaId, pacto.DataInicio, pacto.DataFim);
            pacto.DiasNaoUteis = diasNaoUteis.Result.ToList();

            //Obtem os dados do item
            var item = await ItemCatalogoRepository.ObterAsync(request.ItemCatalogoId);

            //Obtém o tempo previsto para o iem de acordo com a forma de execução e com o tipo do item
            var tempoPrevisto = item.TempoExecucaoPreviamenteDefinido ?
                pacto.ModalidadeExecucaoId == (int)ModalidadeExecucaoEnum.Presencial ? item.TempoExecucaoPresencial.Value : item.TempoExecucaoRemoto.Value :
                request.TempoPrevistoPorItem.Value;

            var modalidaExecucaoId = pacto.ModalidadeExecucaoId;
            if (pacto.ModalidadeExecucaoId == (int)Domain.Enums.ModalidadeExecucaoEnum.Semipresencial)
            {
                modalidaExecucaoId = request.ExecucaoRemota ? (int)Domain.Enums.ModalidadeExecucaoEnum.Teletrabalho : (int)Domain.Enums.ModalidadeExecucaoEnum.Presencial;
            }

            //Cria a atividade
            pacto.AdicionarAtividade(item, request.Quantidade, modalidaExecucaoId, tempoPrevisto, request.Descricao, request.AssuntosId, request.ObjetosId);

            //Altera o pacto de trabalho no banco de dados
            PactoTrabalhoRepository.Atualizar(pacto);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Pacto de trabalho alterado com sucesso.");
            return result;
        }
    }
}
