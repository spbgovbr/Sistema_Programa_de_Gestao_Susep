using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Queries.Abstractions
{
    public interface IPlanoTrabalhoQuery
    {
        Task<IApplicationResult<DadosPaginadosViewModel<PlanoTrabalhoViewModel>>> ObterPorFiltroAsync(PlanoTrabalhoFiltroRequest request);
        Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterAtualAsync(UsuarioLogadoRequest request);
        Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterEmHabiliacaoAsync(UsuarioLogadoRequest request);
        Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterPorChaveAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterTermoAceitePorChaveAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoAtividadeViewModel>>> ObterAtividadesPlanoAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoMetaViewModel>>> ObterMetasPlanoAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoReuniaoViewModel>>> ObterReunioesPlanoAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoCustoViewModel>>> ObterCustosPlanoAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoHistoricoViewModel>>> ObterHistoricoPorPlanoAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoAtividadeCandidatoViewModel>>> ObterCandidatosAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoAtividadeCandidatoViewModel>>> ObterCandidatosPorAtividadeAsync(Guid planoTrabalhoAtividadeId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoPessoaModalidadeViewModel>>> ObterPessoasModalidadesAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoObjetoViewModel>>> ObterObjetosPlanoAsync(Guid planoTrabalhoId);
        Task<IApplicationResult<PlanoTrabalhoObjetoViewModel>> ObterObjetoPlanoByIdAsync(Guid planoTrabalhoObjetoId);
        Task<IApplicationResult<IEnumerable<PlanoTrabalhoObjetoPactoAtividadeViewModel>>> ObterObjetosPlanoAssociadosOuNaoAAtividadesDoPactoAsync(Guid planoTrabalhoId, Guid pactoTrabalhoAtividadeId);
    }
}
