using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.ItemCatalogo
{
    public class ExcluirItemCatalogoCommandHandler : IRequestHandler<ExcluirItemCatalogoCommand, IActionResult>
    {
        private IItemCatalogoQuery ItemCatalogoQuery { get; }        
        private IItemCatalogoRepository ItemCatalogoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public ExcluirItemCatalogoCommandHandler(
            IItemCatalogoQuery catalogoQuery, 
            IItemCatalogoRepository catalogoRepository, 
            IUnitOfWork unitOfWork)
        {
            ItemCatalogoQuery = catalogoQuery;
            ItemCatalogoRepository = catalogoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(ExcluirItemCatalogoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            //Adiciona o catalogo no banco de dados
            await ItemCatalogoRepository.ExcluirAsync(request.ItemCatalogoId);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Item de catálogo excluído com sucesso.");
            return result;
        }
    }
}
