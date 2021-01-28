using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Commands.Catalogo;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Susep.SISRH.WebApi.Controllers
{
    /// <summary>
    /// Operações com os catálogos
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        private IMediator Mediator { get; }
        private readonly ICatalogoQuery CatalogoQuery;

        /// <summary>
        /// Contrutor da classe
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="catalogoQuery"></param>
        public CatalogoController(IMediator mediator, ICatalogoQuery catalogoQuery)
        {
            Mediator = mediator;
            CatalogoQuery = catalogoQuery;
        }

        /// <summary>
        /// Obtém os catálogo cadastrados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<CatalogoViewModel>>))]
        public async Task<IActionResult> GetAll([FromQuery] CatalogoFiltroRequest request)
            => await CatalogoQuery.ObterPorFiltroAsync(request);


        /// <summary>
        /// Obtém o catálogo de uma unidade
        /// </summary>
        /// <param name="unidadeId"></param>
        /// <returns></returns>
        [HttpGet("{unidadeId}"), Produces("application/json", Type = typeof(IApplicationResult<CatalogoViewModel>))]
        public async Task<IActionResult> GetByUnidadeId([FromRoute] Int32 unidadeId)
            => await CatalogoQuery.ObterPorUnidadeAsync(unidadeId);


        /// <summary>
        /// Obtém o catálogo de uma unidade
        /// </summary>
        /// <param name="unidadeId"></param>
        /// <returns></returns>
        [HttpGet("{unidadeId}/itens"), Produces("application/json", Type = typeof(IApplicationResult<CatalogoViewModel>))]
        public async Task<IActionResult> GetItensByUnidadeId([FromRoute] Int32 unidadeId)
            => await CatalogoQuery.ObterItensPorUnidadeAsync(unidadeId);


        /// <summary>
        /// Cadastra um catálogo
        /// </summary>
        /// <param name="command">Dados do  de catálogo a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost(), Produces("application/json", Type = typeof(IApplicationResult<Guid>))]
        public async Task<IActionResult> PostCatalogo([FromBody] CadastrarCatalogoCommand command)
            => await Mediator.Send(command);


        /// <summary>
        /// Altera os dados de um catálogo
        /// </summary>
        /// <param name="catalogoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{Catalogoid}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutCatalogo([FromRoute] Guid catalogoid, [FromBody] AlterarCatalogoCommand command)
        {
            command.CatalogoId = catalogoid;
            return await Mediator.Send(command);
        }


        /// <summary>
        /// Exclui um catálogo
        /// </summary>
        /// <param name="catalogoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{Catalogoid}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteCatalogo([FromRoute] Guid catalogoid, [FromQuery] ExcluirCatalogoCommand command)
        {
            command.CatalogoId = catalogoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Associar um item ao catalogo
        /// </summary>
        /// <param name="catalogoId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{catalogoid}/item"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostCatalogoItem([FromRoute] Guid catalogoId, [FromBody] CadastrarCatalogoItemCatalogoCommand command)
        {
            command.CatalogoId = catalogoId;
            return await Mediator.Send(command);

        }

        /// <summary>
        /// Exclui um item de catálogo
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{catalogoid}/item/{itemCatalogoId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteItemCatalogo([FromRoute] ExcluirCatalogoItemCatalogoCommand command)
              => await Mediator.Send(command);
    }
}