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
    public interface IEstruturaOrganizacionalQuery
    {

        Task<IApplicationResult<PessoaPerfilViewModel>> ObterPerfilPessoaAsync(UsuarioLogadoRequest request);

        Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterUnidadesAtivasDadosComboAsync(UsuarioLogadoRequest request);
        Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterUnidadesComPlanoTrabalhoDadosComboAsync(UsuarioLogadoRequest request);
        Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterComCatalogoCadastradoComboAsync(UsuarioLogadoRequest request);
        
        Task<IApplicationResult<IEnumerable<DadosComboViewModel>>> ObterPessoasComPactoTrabalhoDadosComboAsync(UsuarioLogadoRequest request);

        Task<IApplicationResult<DadosPaginadosViewModel<PlanoTrabalhoViewModel>>> ObterPlanoTrabalhoPorFiltroAsync(PlanoTrabalhoFiltroRequest request);
        Task<IApplicationResult<PlanoTrabalhoViewModel>> ObterPlanoTrabalhoPorChaveAsync(PlanoTrabalhoRequest request);

        Task<IApplicationResult<IEnumerable<PactoTrabalhoViewModel>>> ObterPactosTrabalhoPorPlanoAsync(PlanoTrabalhoRequest request);
        Task<IApplicationResult<DadosPaginadosViewModel<PactoTrabalhoViewModel>>> ObterPactoTrabalhoPorFiltroAsync(PactoTrabalhoFiltroRequest request);
        Task<IApplicationResult<PactoTrabalhoViewModel>> ObterPactoTrabalhoPorChaveAsync(PactoTrabalhoRequest request);

    }
}
