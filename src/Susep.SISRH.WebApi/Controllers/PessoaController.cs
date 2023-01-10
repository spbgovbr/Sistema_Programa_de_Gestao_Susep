using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Commands.Agendamento;
using Susep.SISRH.Application.Commands.ItemCatalogo;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Application.Queries.Concrete;
using Susep.SISRH.Application.Requests;
using Susep.SISRH.Application.ViewModels;
using Susep.SISRH.Domain.Enums;
using SUSEP.Framework.Result.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Susep.SISRH.WebApi.Controllers
{
    /// <summary>
    /// Operações com os planos de trabalho
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private IMediator Mediator { get; }
        private readonly IPessoaQuery PessoaQuery;
        private readonly IEstruturaOrganizacionalQuery EstruturaOrganizacionalQuery;

        /// <summary>
        /// Contrutor da classe
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="pessoaQuery"></param>    
        /// <param name="estruturaOrganizacionalQuery"></param>     
        public PessoaController(
            IMediator mediator, 
            IPessoaQuery pessoaQuery, 
            IEstruturaOrganizacionalQuery estruturaOrganizacionalQuery)
        {
            Mediator = mediator;
            PessoaQuery = pessoaQuery;
            EstruturaOrganizacionalQuery = estruturaOrganizacionalQuery;
        }


        /// <summary>
        /// Obtém os planos de trabalho para o dashboard da pessoa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("dashboardPlanos"), Produces("application/json", Type = typeof(IApplicationResult<DashboardViewModel>))]
        public async Task<IActionResult> GetDashboardPlanos([FromQuery] UsuarioLogadoRequest request)
            => await PessoaQuery.ObterDashboardPlanosAsync(request);

        /// <summary>
        /// Obtém os pactos de trabalho para o dashboard da pessoa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("dashboardPactos"), Produces("application/json", Type = typeof(IApplicationResult<DashboardViewModel>))]
        public async Task<IActionResult> GetDashboardPactos([FromQuery] UsuarioLogadoRequest request)
            => await PessoaQuery.ObterDashboardPactosAsync(request);

        /// <summary>
        /// Obtém as pendências para o dashboard da pessoa
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("dashboardPendencias"), Produces("application/json", Type = typeof(IApplicationResult<DashboardViewModel>))]
        public async Task<IActionResult> GetDashboardPendencias([FromQuery] UsuarioLogadoRequest request)
            => await PessoaQuery.ObterDashboardPendenciasAsync(request);

        /// <summary>
        /// Obtém as pessoas cadastradas
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PessoaViewModel>>))]
        public async Task<IActionResult> GetAll([FromQuery] PessoaFiltroRequest request)
            => await PessoaQuery.ObterPorFiltroAsync(request);


        /// <summary>
        /// Obtém o perfil do usuário logado pessoas cadastradas
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("perfil"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PessoaViewModel>>))]
        public async Task<IActionResult> GetPerfilAll([FromQuery] UsuarioLogadoRequest request)
            => await EstruturaOrganizacionalQuery.ObterPerfilPessoaAsync(request);


        /// <summary>
        /// Obtém uma pessoa por ID
        /// </summary>
        /// <param name="pessoaId"></param>
        /// <returns></returns>
        [HttpGet("{pessoaId}"), Produces("application/json", Type = typeof(IApplicationResult<PessoaViewModel>))]
        public async Task<IActionResult> GetByPessoaId([FromRoute] Int32 pessoaId)
            => await PessoaQuery.ObterDetalhesPorChaveAsync(pessoaId);

        /// <summary>
        /// Obtém os feriados por pessoa e período
        /// </summary>
        /// <param name="pessoaId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{pessoaId}/feriados"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<DateTime>>))]
        public async Task<IActionResult> GetFeriados([FromRoute] Int64 pessoaId, [FromQuery] FeriadoBuscaViewModel request)
            => await PessoaQuery.ObterDiasNaoUteisAsync(pessoaId, request.DataInicio, request.DataFim);

        /// <summary>
        /// Obtém as pessoas que possuem pelo menos um pacto de trabalho cadastrado para preenchimento de combos
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("compactotrabalho"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<DadosComboViewModel>>))]
        public async Task<IActionResult> GetComPlanoTrabalhoDadosCombo([FromRoute]UsuarioLogadoRequest request)
            => await EstruturaOrganizacionalQuery.ObterPessoasComPactoTrabalhoDadosComboAsync(request);



        #region Agendamentos

        /// <summary>
        /// Obtém os agendamentos das pessoas no período informado
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("agendamentos"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<AgendamentoPresencialViewModel>>))]
        public async Task<IActionResult> GetAgendamentosAsync([FromQuery] AgendamentoFiltroRequest request)
            => await EstruturaOrganizacionalQuery.ObterAgendamentosAsync(request);

        /// <summary>
        /// Cadastra um agendamento presencial
        /// </summary>
        /// <param name="command">Dados do item de catálogo a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost("agendamentos"), Produces("application/json", Type = typeof(IApplicationResult<Guid>))]
        public async Task<IActionResult> PostAgendamento([FromBody] CadastrarAgendamentoCommand command)
            => await Mediator.Send(command);


        /// <summary>
        /// Exclui um item de catálogo do corretor logado
        /// </summary>
        /// <param name="agendamentoId"></param>
        /// <returns></returns>
        [HttpDelete("agendamentos/{agendamentoId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteItemCatalogo([FromRoute] Guid agendamentoId)
        {
            var command = new ExcluirAgendamentoCommand()
            {
                AgendamentoId = agendamentoId
            };
            return await Mediator.Send(command);
        }

        #endregion

    }
}