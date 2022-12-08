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
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoViewModel>>> ObterDashboardPlanosAsync(UsuarioLogadoRequest request);
        Task<IApplicationResult<IEnumerable<PactoTrabalhoViewModel>>> ObterDashboardPactosAsync(UsuarioLogadoRequest request);
        Task<IApplicationResult<IEnumerable<PactoTrabalhoSolicitacaoViewModel>>> ObterDashboardPendenciasAsync(UsuarioLogadoRequest request);

        Task<IApplicationResult<DadosPaginadosViewModel<PessoaViewModel>>> ObterPorFiltroAsync(PessoaFiltroRequest request);
        Task<IApplicationResult<PessoaViewModel>> ObterPorChaveAsync(Int64 pessoaId);
        Task<IApplicationResult<IEnumerable<DateTime>>> ObterDiasNaoUteisAsync(Int64 pessoaId, DateTime dataInicio, DateTime dataFim);
        Task<IApplicationResult<PessoaViewModel>> ObterDetalhesPorChaveAsync(Int64 pessoaId);
        Task<IApplicationResult<IEnumerable<PessoaViewModel>>> ObterComPactoTrabalhoAsync();

        Task<IApplicationResult<IEnumerable<AgendamentoPresencialViewModel>>> ObterAgendamentosAsync(AgendamentoFiltroRequest request, Boolean isGestor);

        Task<IApplicationResult<IEnumerable<PactoTrabalhoViewModel>>> ObterPactosTrabalhoEmExecucaoAsync(Int64 pessoaId);

    }
}
