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
    public interface IPessoaQuery
    {
        Task<IApplicationResult<DashboardViewModel>> ObterDashboardAsync(UsuarioLogadoRequest request);

        Task<IApplicationResult<DadosPaginadosViewModel<PessoaViewModel>>> ObterPorFiltroAsync(PessoaFiltroRequest request);
        Task<IApplicationResult<PessoaViewModel>> ObterPorChaveAsync(Int64 pessoaId);
        Task<IApplicationResult<IEnumerable<DateTime>>> ObterDiasNaoUteisAsync(Int64 pessoaId, DateTime dataInicio, DateTime dataFim);
        Task<IApplicationResult<PessoaViewModel>> ObterDetalhesPorChaveAsync(Int64 pessoaId);
        Task<IApplicationResult<IEnumerable<PessoaViewModel>>> ObterComPactoTrabalhoAsync();

    }
}
