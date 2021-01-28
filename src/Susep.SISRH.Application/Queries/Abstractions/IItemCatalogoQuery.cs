using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Abstractions
{
    public interface IItemCatalogoQuery
    {
        Task<IApplicationResult<DadosPaginadosViewModel<ItemCatalogoViewModel>>> ObterPorFiltroAsync(ItemCatalogoFiltroRequest request);
        Task<IApplicationResult<ItemCatalogoViewModel>> ObterPorChaveAsync(Guid itemCatalogoid);
    }
}
