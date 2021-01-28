using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Result.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Abstractions
{
    public interface IAPIExtracaoOrgaoCentralQuery
    {

        Task<IApplicationResult<IEnumerable<APIPlanoTrabalhoViewModel>>> ObterPlanosTrabalhoAsync();
        
    }
}
