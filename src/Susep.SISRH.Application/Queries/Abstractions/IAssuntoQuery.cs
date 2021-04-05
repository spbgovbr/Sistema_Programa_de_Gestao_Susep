using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Messages.Concrete.Request;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Abstractions
{
    public interface IAssuntoQuery
    {
        Task<IApplicationResult<DadosPaginadosViewModel<AssuntoViewModel>>> ObterPorFiltroAsync(AssuntoFiltroRequest request);
        Task<IApplicationResult<IEnumerable<AssuntoViewModel>>> ObterAtivosAsync(); 
        Task<IApplicationResult<AssuntoEdicaoViewModel>> ObterPorIdAsync(Guid id);
        Task<IApplicationResult<IEnumerable<AssuntoViewModel>>> ObterPorTextoAsync(string texto);
        Task<IApplicationResult<IEnumerable<Guid>>> ObterIdsDeTodosOsPaisAsync(Guid assuntoId);
        Task<IApplicationResult<IEnumerable<Guid>>> ObterIdsDeTodosOsFilhosAsync(Guid assuntoId);
        Task<IApplicationResult<bool>> ValorDuplicadoAsync(string valor, Guid? assuntoId);
        
    }
}
