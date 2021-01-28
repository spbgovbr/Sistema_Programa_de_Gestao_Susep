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
    public interface IObjetoQuery
    {
        Task<IApplicationResult<DadosPaginadosViewModel<ObjetoViewModel>>> ObterPorFiltroAsync(ObjetoFiltroRequest request);
        Task<IApplicationResult<ObjetoViewModel>> ObterPorIdAsync(Guid id);
        Task<IApplicationResult<bool>> ChaveDuplicadaAsync(string chave, Guid? objetoId);
        Task<IApplicationResult<IEnumerable<ObjetoViewModel>>> ObterPorTextoAsync(string texto);
    }
}
