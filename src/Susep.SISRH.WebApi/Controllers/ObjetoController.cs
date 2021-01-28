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
    public class ObjetoController : ControllerBase
    {
        private IMediator Mediator { get; }
        private readonly IObjetoQuery ObjetoQuery;

        /// <summary>
        /// Contrutor da classe
        /// </summary>
        /// <param name="mediator"></param>    
        /// <param name="objetoQuery"></param>    
        public ObjetoController(IMediator mediator, IObjetoQuery objetoQuery)
        {
            Mediator = mediator;
            ObjetoQuery = objetoQuery;
        }

        /// <summary>
        /// Obtém os objetos cadastrados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<ObjetoViewModel>>))]
        public async Task<IActionResult> GetAll([FromQuery] ObjetoFiltroRequest request)
        {
            return await ObjetoQuery.ObterPorFiltroAsync(request);
        }

        /// <summary>
        /// Obtém o objeto por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}"), Produces("application/json", Type = typeof(IApplicationResult<ObjetoViewModel>))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            return await ObjetoQuery.ObterPorIdAsync(id);
        }

        /// <summary>
        /// Obtém os objetos cadastrados por texto
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        [HttpGet("texto"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<ObjetoViewModel>>))]
        public async Task<IActionResult> GetByTexto([FromQuery] string texto)
        {
            return await ObjetoQuery.ObterPorTextoAsync(texto);
        }

        /// <summary>
        /// Cadastra um objeto
        /// </summary>
        /// <param name="command">Dados do objeto a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost(), Produces("application/json", Type = typeof(IApplicationResult<Guid>))]
        public async Task<IActionResult> PostObjeto([FromBody] CadastrarObjetoCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera um objeto
        /// </summary>
        /// <param name="command">Dados do objeto a ser cadastrado</param>
        /// <returns></returns>
        [HttpPut(), Produces("application/json", Type = typeof(IApplicationResult<Guid>))]
        public async Task<IActionResult> PutObjeto([FromBody] AlterarObjetoCommand command)
        {
            return await Mediator.Send(command);
        }

    }

}