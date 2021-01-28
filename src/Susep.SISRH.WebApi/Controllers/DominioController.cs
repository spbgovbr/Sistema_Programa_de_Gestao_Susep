using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Queries.Concrete;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Susep.SISRH.WebApi.Controllers
{
    /// <summary>
    /// Operações com os dados de domínio do programa de gestão
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class DominioController : ControllerBase
    {
        private readonly IDominioQuery DominioQuery;

        /// <summary>
        /// Contrutor da classe
        /// </summary>
        /// <param name="dominioQuery"></param>
        public DominioController(IDominioQuery dominioQuery)
        {            
            DominioQuery = dominioQuery;
        }

        /// <summary>
        /// Obtém os catálogo cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("ModalidadeExecucao"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<DominioViewModel>>))]
        public async Task<IActionResult> GetModalidadeExecucao()
            => await DominioQuery.ObterDominioPorClassificacaoAsync(Domain.Enums.ClassificacaoCatalogoDominioEnum.ModalidadeExecucao);


        /// <summary>
        /// Obtém os catálogo cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("FormaCalculoTempoItemCatalogo"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<DominioViewModel>>))]
        public async Task<IActionResult> GetFormaCalculoTempoItemCatalogo()
            => await DominioQuery.ObterDominioPorClassificacaoAsync(Domain.Enums.ClassificacaoCatalogoDominioEnum.FormaCalculoTempoItemCatalogo);


        /// <summary>
        /// Obtém os catálogo cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("SituacaoPlanoTrabalho"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<DominioViewModel>>))]
        public async Task<IActionResult> GetSituacaoPlanoTrabalho()
            => await DominioQuery.ObterDominioPorClassificacaoAsync(Domain.Enums.ClassificacaoCatalogoDominioEnum.SituacaoPlanoTrabalho);



        /// <summary>
        /// Obtém os catálogo cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("SituacaoPactoTrabalho"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<DominioViewModel>>))]
        public async Task<IActionResult> GetSituacaoPactoTrabalho()
            => await DominioQuery.ObterDominioPorClassificacaoAsync(Domain.Enums.ClassificacaoCatalogoDominioEnum.SituacaoPactoTrabalho);



        /// <summary>
        /// Obtém os catálogo cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("SituacaoAtividadePactoTrabalho"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<DominioViewModel>>))]
        public async Task<IActionResult> GetSituacaoAtividadePactoTrabalho()
            => await DominioQuery.ObterDominioPorClassificacaoAsync(Domain.Enums.ClassificacaoCatalogoDominioEnum.SituacaoAtividadePactoTrabalho);

        /// <summary>
        /// Obtém os catálogo cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("CriterioPerfilAtividadePlano"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<DominioViewModel>>))]
        public async Task<IActionResult> GetCriterioPerfilAtividadePlano()
            => await DominioQuery.ObterDominioPorClassificacaoAsync(Domain.Enums.ClassificacaoCatalogoDominioEnum.CriterioPerfilAtividadePlano);



        /// <summary>
        /// Obtém os dominios por situação plano trablaho
        /// </summary>
        /// <returns></returns>
        [HttpGet("SituacaoCandidaturaPlanoTrabalho/Solicitada"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<DominioViewModel>>))]
        public async Task<IActionResult> GetSituacaoCandidaturaPlanoTrabalho()
            => await DominioQuery.ObterDominioPorSituacaoCandidaturaPlanoTrabalhoAsync(Domain.Enums.SituacaoCandidaturaPlanoTrabalhoEnum.Solicitada);


    }


}