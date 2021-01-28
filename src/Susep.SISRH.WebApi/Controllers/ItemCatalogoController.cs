using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Commands.ItemCatalogo;
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
    /// Operações com os itens de catálogo
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class ItemCatalogoController : ControllerBase
    {
        private IMediator Mediator { get; }
        private readonly IItemCatalogoQuery ItemCatalogoQuery;

        /// <summary>
        /// Contrutor da classe
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="itemCatalogoQuery"></param>
        public ItemCatalogoController(IMediator mediator, IItemCatalogoQuery itemCatalogoQuery)
        {
            Mediator = mediator;
            ItemCatalogoQuery = itemCatalogoQuery;
        }

        /// <summary>
        /// Obtém os itens de catálogo cadastrados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<ItemCatalogoViewModel>>))]
        public async Task<IActionResult> GetAll([FromQuery]ItemCatalogoFiltroRequest request)
            => await ItemCatalogoQuery.ObterPorFiltroAsync(request);


        /// <summary>
        /// Obtém os dados de um item de catálogo
        /// </summary>
        /// <param name="itemCatalogoid"></param>
        /// <returns></returns>
        [HttpGet("{itemCatalogoid}"), Produces("application/json", Type = typeof(IApplicationResult<ItemCatalogoViewModel>))]
        public async Task<IActionResult> GetById([FromRoute]Guid itemCatalogoid)
            => await ItemCatalogoQuery.ObterPorChaveAsync(itemCatalogoid);


        /// <summary>
        /// Cadastra um item de catálogo do corretor logado
        /// </summary>
        /// <param name="command">Dados do item de catálogo a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost(), Produces("application/json", Type = typeof(IApplicationResult<Guid>))]
        public async Task<IActionResult> PostItemCatalogo([FromBody] CadastrarItemCatalogoCommand command)
            => await Mediator.Send(command);


        /// <summary>
        /// Altera os dados de um item de catálogo do corretor logado
        /// </summary>
        /// <param name="itemCatalogoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{itemCatalogoid}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutItemCatalogo([FromRoute] Guid itemCatalogoid, [FromBody] AlterarItemCatalogoCommand command)
        {
            command.ItemCatalogoId = itemCatalogoid;
            return await Mediator.Send(command);
        }


        /// <summary>
        /// Exclui um item de catálogo do corretor logado
        /// </summary>
        /// <param name="itemCatalogoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{itemCatalogoid}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteItemCatalogo([FromRoute] Guid itemCatalogoid, [FromQuery] ExcluirItemCatalogoCommand command)
        {
            command.ItemCatalogoId = itemCatalogoid;
            return await Mediator.Send(command);
        }


    }
}