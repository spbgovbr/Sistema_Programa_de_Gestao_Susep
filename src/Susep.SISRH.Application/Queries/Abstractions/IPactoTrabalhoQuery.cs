using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Abstractions
{
    public interface IPactoTrabalhoQuery
    {
        Task<IApplicationResult<DadosPaginadosViewModel<PactoTrabalhoViewModel>>> ObterPorFiltroAsync(PactoTrabalhoFiltroRequest request);
        Task<IApplicationResult<PactoTrabalhoViewModel>> ObterAtualAsync(UsuarioLogadoRequest request);        
        Task<IApplicationResult<PactoTrabalhoViewModel>> ObterPorChaveAsync(Guid pactoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PactoTrabalhoViewModel>>> ObterPorPlanoAsync(Guid pactoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PactoTrabalhoHistoricoViewModel>>> ObterHistoricoPorPactoAsync(Guid pactoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PactoTrabalhoAtividadeViewModel>>> ObterAtividadesPactoAsync(Guid pactoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PactoTrabalhoSolicitacaoViewModel>>> ObterSolicitacoesPactoAsync(Guid pactoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PactoTrabalhoInformacaoViewModel>>> ObterInformacoesPactoAsync(Guid pactoTrabalhoId);
        Task<IApplicationResult<PactoTrabalhoAssuntosParaAssociarViewModel>> ObterAssuntosParaAssociarAsync(Guid pactoTrabalhoId);
        Task<IApplicationResult<IEnumerable<DominioViewModel>>> ObterDeclaracoesNaoRealizadasAsync(Guid pactoTrabalhoId);

    }
}
