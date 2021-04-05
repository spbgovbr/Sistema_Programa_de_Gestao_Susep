using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Susep.SISRH.Application.Commands.Assunto;


namespace Susep.SISRH.WebApi.Controllers
{
    /// <summary>
    /// Operações com os assuntos
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class AssuntoController : ControllerBase
    {
        private IMediator Mediator { get; }
        private readonly IAssuntoQuery AssuntoQuery;

        /// <summary>
        /// Contrutor da classe
        /// </summary>
        /// <param name="mediator"></param>    
        /// <param name="assuntoQuery"></param>    
        public AssuntoController(IMediator mediator, IAssuntoQuery assuntoQuery)
        {
            Mediator = mediator;
            AssuntoQuery = assuntoQuery;
        }

        /// <summary>
        /// Obtém os assuntos cadastrados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<AssuntoViewModel>>))]
        public async Task<IActionResult> GetAll([FromQuery] AssuntoFiltroRequest request)
        {
            return await AssuntoQuery.ObterPorFiltroAsync(request);
        }

        /// <summary>
        /// Obtém os assuntos cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("ativos"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<AssuntoViewModel>>))]
        public async Task<IActionResult> GetAtivos()
        {
            return await AssuntoQuery.ObterAtivosAsync();
        }

        /// <summary>
        /// Obtém o assunto por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}"), Produces("application/json", Type = typeof(IApplicationResult<AssuntoViewModel>))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            return await AssuntoQuery.ObterPorIdAsync(id);
        }

        /// <summary>
        /// Obtém os assuntos cadastrados por texto
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        [HttpGet("texto"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<AssuntoViewModel>>))]
        public async Task<IActionResult> GetByTexto([FromQuery] string texto)
        {
            return await AssuntoQuery.ObterPorTextoAsync(texto);
        }

        /// <summary>
        /// Cadastra um assunto
        /// </summary>
        /// <param name="command">Dados do item de catálogo a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost(), Produces("application/json", Type = typeof(IApplicationResult<Guid>))]
        public async Task<IActionResult> PostAssunto([FromBody] CadastrarAssuntoCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera um assunto
        /// </summary>
        /// <param name="command">Dados do item de catálogo a ser cadastrado</param>
        /// <returns></returns>
        [HttpPut(), Produces("application/json", Type = typeof(IApplicationResult<Guid>))]
        public async Task<IActionResult> PutAssunto([FromBody] AlterarAssuntoCommand command)
        {
            return await Mediator.Send(command);
        }

    }

}