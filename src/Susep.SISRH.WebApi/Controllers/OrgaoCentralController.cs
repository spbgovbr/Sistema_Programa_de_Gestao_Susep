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
    public class OrgaoCentralController : ControllerBase
    {
        private IMediator Mediator { get; }
        private readonly IAPIExtracaoOrgaoCentralQuery APIExtracaoOrgaoCentralQuery;

        /// <summary>
        /// Contrutor da classe
        /// </summary>
        /// <param name="mediator"></param>    
        /// <param name="apiExtracaoOrgaoCentralQuery"></param>    
        public OrgaoCentralController(IMediator mediator, IAPIExtracaoOrgaoCentralQuery apiExtracaoOrgaoCentralQuery)
        {
            Mediator = mediator;
            APIExtracaoOrgaoCentralQuery = apiExtracaoOrgaoCentralQuery;
        }

        /// <summary>
        /// Obtém os assuntos cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet("planotrabalho"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<APIPlanoTrabalhoViewModel>>))]
        public async Task<IActionResult> GetAll()
        {
            return await APIExtracaoOrgaoCentralQuery.ObterPlanosTrabalhoAsync();
        }


    }

}