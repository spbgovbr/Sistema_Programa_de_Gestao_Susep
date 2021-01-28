using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.Catalogo
{
    public class ExcluirCatalogoCommandHandler : IRequestHandler<ExcluirCatalogoCommand, IActionResult>
    {
        private ICatalogoQuery CatalogoQuery { get; }        
        private ICatalogoRepository CatalogoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public ExcluirCatalogoCommandHandler(
            ICatalogoQuery catalogoQuery, 
            ICatalogoRepository catalogoRepository, 
            IUnitOfWork unitOfWork)
        {
            CatalogoQuery = catalogoQuery;
            CatalogoRepository = catalogoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(ExcluirCatalogoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            //Exclui o catalogo no banco de dados
            await CatalogoRepository.ExcluirAsync(request.CatalogoId);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Catalogo excluído com sucesso.");
            return result;
        }
    }
}
