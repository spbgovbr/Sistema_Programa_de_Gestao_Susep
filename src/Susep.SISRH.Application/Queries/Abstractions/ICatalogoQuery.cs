using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Messages.Concrete.Request;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Abstractions
{
    public interface ICatalogoQuery
    {
        Task<IApplicationResult<DadosPaginadosViewModel<CatalogoViewModel>>> ObterPorFiltroAsync(CatalogoFiltroRequest request);
        Task<IApplicationResult<CatalogoViewModel>> ObterPorChaveAsync(Guid catalogoid);
        Task<IApplicationResult<CatalogoViewModel>> ObterPorUnidadeAsync(Int32 unidadeId);
        Task<IApplicationResult<IEnumerable<ItemCatalogoViewModel>>> ObterItensPorUnidadeAsync(Int32 unidadeId);
        
    }
}
