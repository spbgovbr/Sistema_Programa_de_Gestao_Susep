using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class ExcluirPactoTrabalhoAtividadeCommandHandler : IRequestHandler<ExcluirPactoTrabalhoAtividadeCommand, IActionResult>
    {
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public ExcluirPactoTrabalhoAtividadeCommandHandler(            
            IPactoTrabalhoRepository planoTrabalhoRepository, 
            IUnitOfWork unitOfWork)
        {
            PactoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(ExcluirPactoTrabalhoAtividadeCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);            

            //Monta o objeto com os dados do catalogo
            var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

            //Remove a atividade
            pacto.RemoverAtividade(request.PactoTrabalhoAtividadeId);

            //Altera o pacto de trabalho no banco de dados
            PactoTrabalhoRepository.Atualizar(pacto);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Pacto de trabalho alterado com sucesso.");
            return result;
        }
    }
}
