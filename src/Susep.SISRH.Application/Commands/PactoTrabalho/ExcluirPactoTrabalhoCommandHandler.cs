using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.PactoTrabalho
{
    public class ExcluirPactoTrabalhoCommandHandler : IRequestHandler<ExcluirPactoTrabalhoCommand, IActionResult>
    {
        private IPactoTrabalhoRepository PactoTrabalhoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public ExcluirPactoTrabalhoCommandHandler(            
            IPactoTrabalhoRepository planoTrabalhoRepository, 
            IUnitOfWork unitOfWork)
        {
            PactoTrabalhoRepository = planoTrabalhoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(ExcluirPactoTrabalhoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);            

            //Monta o objeto com os dados do catalogo
            var pacto = await PactoTrabalhoRepository.ObterAsync(request.PactoTrabalhoId);

            if (pacto.SituacaoId >= (int)Domain.Enums.SituacaoPactoTrabalhoEnum.EmExecucao)
            {
                result.Validations = new List<string> { "Não é é possível excluir um plano que já tenha iniciado a execução" };
                return result;
            }

            //Altera o pacto de trabalho no banco de dados
            PactoTrabalhoRepository.Excluir(pacto);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Pacto de trabalho alterado com sucesso.");
            return result;
        }
    }
}
