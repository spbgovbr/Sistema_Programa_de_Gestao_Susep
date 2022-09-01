using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class RegistrarRealizacaoDeclaracaoPactoTrabalhoCommandHandler : IRequestHandler<RegistrarRealizacaoDeclaracaoPactoTrabalhoCommand, IActionResult>
    {
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public RegistrarRealizacaoDeclaracaoPactoTrabalhoCommandHandler(
            IPactoTrabalhoRepository planoTrabalhoRepository,
            IUnitOfWork unitOfWork)
        {
            PactoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(RegistrarRealizacaoDeclaracaoPactoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            //Monta o objeto com os dados do catalogo
            var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

            pacto.RegistrarRealizacaoDeclaracao(request.DeclaracaoId, pacto.PessoaId.ToString());

            //Altera o pacto de trabalho no banco de dados
            PactoTrabalhoRepository.Atualizar(pacto);

            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Registro da visualização da declaração feito com sucesso.");
            return result;
        }
    }
}
