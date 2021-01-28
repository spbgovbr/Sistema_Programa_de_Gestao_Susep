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
    public interface IUnidadeQuery
    {
        Task<IApplicationResult<IEnumerable<UnidadeViewModel>>> ObterAtivasAsync();
        Task<IApplicationResult<IEnumerable<UnidadeViewModel>>> ObterComPlanoTrabalhoAsync();
        Task<IApplicationResult<UnidadeViewModel>> ObterPorChaveAsync(Int64 unidadeId);
        Task<IApplicationResult<IEnumerable<DateTime>>> ObterFeriadosPorUnidadeAsync(Int64 unidadeId, DateTime dataInicio, DateTime dataFim);
        Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterModalidadesExecucaoAsync(Int64 unidadeId);
        Task<IApplicationResult<IEnumerable<PessoaViewModel>>> ObterPessoasAsync(Int64 unidadeId);
        Task<IApplicationResult<IEnumerable<PessoaViewModel>>> ObterChefesAsync(String siglaCompletaunidade);
        Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterPessoasDadosComboAsync(Int64 unidadeId);
        Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterSubordinadasAsync(Int64 unidadeId);        
        Task<IApplicationResult<IEnumerable<UnidadeViewModel>>> ObterComCatalogoCadastradoComboAsync();
        Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterSemCatalogoCadastradoComboAsync();
    }
}
