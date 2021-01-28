using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class AlterarAndamentoPactoTrabalhoAtividadeCommandHandler : IRequestHandler<AlterarAndamentoPactoTrabalhoAtividadeCommand, IActionResult>
    {

        private IItemCatalogoRepository ItemCatalogoRepository { get; }
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IPessoaQuery PessoaQuery { get; }
        private IUnitOfWork UnitOfWork { get; }

        public AlterarAndamentoPactoTrabalhoAtividadeCommandHandler(
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

        public async Task<IActionResult> Handle(AlterarAndamentoPactoTrabalhoAtividadeCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            //Monta os dados do pacto de trabalho
            var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

            //Obtém os dias não úteis da pessoa
            var diasNaoUteis = await PessoaQuery.ObterDiasNaoUteisAsync(pacto.PessoaId, pacto.DataInicio, pacto.DataFim);
            pacto.DiasNaoUteis = diasNaoUteis.Result.ToList();

            //Altera a atividade
            pacto.AlterarAndamentoAtividade(request.PactoTrabalhoAtividadeId, request.SituacaoId, request.DataInicio, request.DataFim, request.TempoRealizado, consideracoes: request.Consideracoes);

            //Altera o pacto de trabalho no banco de dados
            PactoTrabalhoRepository.Atualizar(pacto);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Pacto de trabalho alterado com sucesso.");
            return result;
        }
    }
}
