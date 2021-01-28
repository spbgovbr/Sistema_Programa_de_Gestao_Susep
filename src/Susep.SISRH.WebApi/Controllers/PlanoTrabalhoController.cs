using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Commands.PlanoTrabalho;
using Susep.SISRH.Application.Queries.Abstractions;
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
    public class PlanoTrabalhoController : ControllerBase
    {
        private IMediator Mediator { get; }
        private readonly IPlanoTrabalhoQuery PlanoTrabalhoQuery;
        private readonly IPactoTrabalhoQuery PactoTrabalhoQuery;
        private readonly IEstruturaOrganizacionalQuery EstruturaOrganizacionalQuery;

        /// <summary>
        /// Contrutor da classe
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="planoTrabalhoQuery"></param>
        /// <param name="pactoTrabalhoQuery"></param>
        /// <param name="estruturaOrganizacionalQuery"></param>
        public PlanoTrabalhoController(IMediator mediator, 
            IPlanoTrabalhoQuery planoTrabalhoQuery,
            IPactoTrabalhoQuery pactoTrabalhoQuery, 
            IEstruturaOrganizacionalQuery estruturaOrganizacionalQuery)
        {
            Mediator = mediator;
            PlanoTrabalhoQuery = planoTrabalhoQuery;
            PactoTrabalhoQuery = pactoTrabalhoQuery;
            EstruturaOrganizacionalQuery = estruturaOrganizacionalQuery;
        }

        /// <summary>
        /// Obtém os planos de trabalho cadastrados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PlanoTrabalhoViewModel>>))]
        public async Task<IActionResult> GetAll([FromQuery]PlanoTrabalhoFiltroRequest request)
            => await EstruturaOrganizacionalQuery.ObterPlanoTrabalhoPorFiltroAsync(request);


        /// <summary>
        /// Obtém os dados de um plano de trabalho
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}"), Produces("application/json", Type = typeof(IApplicationResult<PlanoTrabalhoViewModel>))]
        public async Task<IActionResult> GetById([FromRoute]PlanoTrabalhoRequest request)
            => await EstruturaOrganizacionalQuery.ObterPlanoTrabalhoPorChaveAsync(request);

        /// <summary>
        /// Obtém o plano atual na unidade do usuário logado
        /// </summary>
        /// <returns></returns>
        [HttpGet("atual"), Produces("application/json", Type = typeof(IApplicationResult<PlanoTrabalhoViewModel>))]
        public async Task<IActionResult> GetAtual([FromQuery]UsuarioLogadoRequest request)
            => await PlanoTrabalhoQuery.ObterAtualAsync(request);

        /// <summary>
        /// Obtém o plano em habilitação na unidade do usuário logado
        /// </summary>
        /// <returns></returns>
        [HttpGet("habilitacao"), Produces("application/json", Type = typeof(IApplicationResult<PlanoTrabalhoViewModel>))]
        public async Task<IActionResult> GetHabilitacao([FromQuery] UsuarioLogadoRequest request)
            => await PlanoTrabalhoQuery.ObterEmHabiliacaoAsync(request);


        /// <summary>
        /// Obtém o plano em habilitação na unidade do usuário logado
        /// </summary>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/termoAceite"), Produces("application/json", Type = typeof(IApplicationResult<PlanoTrabalhoViewModel>))]
        public async Task<IActionResult> GetTermoAceite([FromRoute] Guid planoTrabalhoid)
            => await PlanoTrabalhoQuery.ObterTermoAceitePorChaveAsync(planoTrabalhoid);


        /// <summary>
        /// Cadastra um plano de trabalho do corretor logado
        /// </summary>
        /// <param name="command">Dados do plano de trabalho a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost(), Produces("application/json", Type = typeof(IApplicationResult<Guid>))]
        public async Task<IActionResult> PostPlanoTrabalho([FromBody] CadastrarPlanoTrabalhoCommand command)
            => await Mediator.Send(command);


        /// <summary>
        /// Altera os dados de um plano de trabalho do corretor logado
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{planoTrabalhoid}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutPlanoTrabalho([FromRoute] Guid planoTrabalhoid, [FromBody] AlterarPlanoTrabalhoCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            return await Mediator.Send(command);
        }


        /// <summary>
        /// Obtém os pactos de trabalho de um plano de trabalho
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/pactosTrabalho"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PactoTrabalhoViewModel>>))]
        public async Task<IActionResult> GetPactosById([FromRoute]Guid planoTrabalhoid, [FromQuery] PlanoTrabalhoRequest request)
        {
            request.PlanoTrabalhoId = planoTrabalhoid;
            return await EstruturaOrganizacionalQuery.ObterPactosTrabalhoPorPlanoAsync(request);
        }


        /// <summary>
        /// Obtém o histórico de um plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/historico"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PactoTrabalhoHistoricoViewModel>>))]
        public async Task<IActionResult> GetHistoricoById([FromRoute]Guid planoTrabalhoid)
            => await PlanoTrabalhoQuery.ObterHistoricoPorPlanoAsync(planoTrabalhoid);



        #region Fluxo

        /// <summary>
        /// Altera a situação do plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{planoTrabalhoid}/situacao"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> AlterarSituacao([FromRoute]Guid planoTrabalhoid, [FromBody]AlterarSituacaoPlanoTrabalhoCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            return await Mediator.Send(command);
        }

        #endregion

        #region Atividades

        /// <summary>
        /// Obtém as atividades de um plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/atividades"), Produces("application/json", Type = typeof(IApplicationResult<PlanoTrabalhoAtividadeViewModel>))]
        public async Task<IActionResult> GetAtividadesById([FromRoute]Guid planoTrabalhoid)
            => await PlanoTrabalhoQuery.ObterAtividadesPlanoAsync(planoTrabalhoid);


        /// <summary>
        /// Adiciona uma atividade no plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{planoTrabalhoid}/atividades"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostAtividadePlano([FromRoute]Guid planoTrabalhoid, [FromBody]CadastrarPlanoTrabalhoAtividadeCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera uma atividade do plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="planoTrabalhoAtividadeId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{planoTrabalhoid}/atividades/{planoTrabalhoAtividadeId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutAtividadePlano([FromRoute]Guid planoTrabalhoid, [FromRoute]Guid planoTrabalhoAtividadeId, [FromBody]AlterarPlanoTrabalhoAtividadeCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            command.PlanoTrabalhoAtividadeId = planoTrabalhoAtividadeId;
            
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Exclui uma atividade do plano
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{planoTrabalhoid}/atividades/{planoTrabalhoAtividadeId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteAtividadePlano([FromRoute]ExcluirPlanoTrabalhoAtividadeCommand command)
            => await Mediator.Send(command);


        #region Candidatos

        /// <summary>
        /// Obtém os candidatos a vagas em um plano
        /// </summary>
        /// <param name="planoTrabalhoId"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/atividades/candidato"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PlanoTrabalhoAtividadeCandidatoViewModel>>))]
        public async Task<IActionResult> GetCandidatosPlano([FromRoute]Guid planoTrabalhoId)
        {
            return await PlanoTrabalhoQuery.ObterCandidatosAsync(planoTrabalhoId);
        }

        /// <summary>
        /// Obtém os candidatos a vagas em uma atividade de um plano
        /// </summary>
        /// <param name="planoTrabalhoAtividadeId"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/atividades/{planoTrabalhoAtividadeId}/candidato"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PlanoTrabalhoAtividadeCandidatoViewModel>>))]
        public async Task<IActionResult> GetCandidatoAtividadePlano([FromRoute]Guid planoTrabalhoAtividadeId)
        {
            return await PlanoTrabalhoQuery.ObterCandidatosPorAtividadeAsync(planoTrabalhoAtividadeId);
        }

        /// <summary>
        /// Registra a candidatura de um servidor a uma vaga em uma atividade de um plano
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{planoTrabalhoid}/atividades/{planoTrabalhoAtividadeId}/candidato"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostCandidatoAtividadePlano([FromRoute]CandidatarPlanoTrabalhoAtividadeCommand command)
        {
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera a situação da candidatura de um servidor a uma vaga em uma atividade de um plano
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="planoTrabalhoAtividadeId"></param>
        /// <param name="planoTrabalhoAtividadeCandidatoId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{planoTrabalhoid}/atividades/{planoTrabalhoAtividadeId}/candidato/{planoTrabalhoAtividadeCandidatoId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutCandidatoAtividadePlano([FromRoute]Guid planoTrabalhoid, [FromRoute]Guid planoTrabalhoAtividadeId, [FromRoute]Guid planoTrabalhoAtividadeCandidatoId, [FromBody]AtualizarCandidaturaPlanoTrabalhoAtividadeCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            command.PlanoTrabalhoAtividadeId = planoTrabalhoAtividadeId;
            command.PlanoTrabalhoAtividadeCandidatoId = planoTrabalhoAtividadeCandidatoId;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Obtém a modalidade de execução de uma pessoa em um determinado plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/pessoasModalidadesExecucao"), Produces("application/json", Type = typeof(IApplicationResult<DadosComboViewModel>))]
        public async Task<IActionResult> GetPessoasModalidades([FromRoute]Guid planoTrabalhoid)
            => await PlanoTrabalhoQuery.ObterPessoasModalidadesAsync(planoTrabalhoid);

        #endregion

        #endregion

        #region Metas

        /// <summary>
        /// Obtém as metas de um plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/metas"), Produces("application/json", Type = typeof(IApplicationResult<PlanoTrabalhoMetaViewModel>))]
        public async Task<IActionResult> GetMetasById([FromRoute]Guid planoTrabalhoid)
            => await PlanoTrabalhoQuery.ObterMetasPlanoAsync(planoTrabalhoid);


        /// <summary>
        /// Adiciona uma meta no plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{planoTrabalhoid}/metas"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostMetaPlano([FromRoute]Guid planoTrabalhoid, [FromBody]CadastrarPlanoTrabalhoMetaCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera uma meta do plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="planoTrabalhoMetaId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{planoTrabalhoid}/metas/{planoTrabalhoMetaId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutMetaPlano([FromRoute]Guid planoTrabalhoid, [FromRoute]Guid planoTrabalhoMetaId, [FromBody]AlterarPlanoTrabalhoMetaCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            command.PlanoTrabalhoMetaId = planoTrabalhoMetaId;

            return await Mediator.Send(command);
        }


        /// <summary>
        /// Exclui uma reunião do plano
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{planoTrabalhoid}/metas/{planoTrabalhoMetaId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteMetaPlano([FromRoute]ExcluirPlanoTrabalhoMetaCommand command)
            => await Mediator.Send(command);

        #endregion

        #region Reuniões

        /// <summary>
        /// Obtém as reuniões de um plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/reunioes"), Produces("application/json", Type = typeof(IApplicationResult<PlanoTrabalhoReuniaoViewModel>))]
        public async Task<IActionResult> GetReunioesById([FromRoute]Guid planoTrabalhoid)
            => await PlanoTrabalhoQuery.ObterReunioesPlanoAsync(planoTrabalhoid);


        /// <summary>
        /// Adiciona uma reuniao no plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{planoTrabalhoid}/reunioes"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostReuniaoPlano([FromRoute]Guid planoTrabalhoid, [FromBody]CadastrarPlanoTrabalhoReuniaoCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera uma reuniao do plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="planoTrabalhoReuniaoId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{planoTrabalhoid}/reunioes/{planoTrabalhoReuniaoId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutReuniaoPlano([FromRoute]Guid planoTrabalhoid, [FromRoute]Guid planoTrabalhoReuniaoId, [FromBody]AlterarPlanoTrabalhoReuniaoCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            command.PlanoTrabalhoReuniaoId = planoTrabalhoReuniaoId;

            return await Mediator.Send(command);
        }


        /// <summary>
        /// Exclui uma reunião do plano
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{planoTrabalhoid}/reunioes/{planoTrabalhoReuniaoId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteReuniaoPlano([FromRoute]ExcluirPlanoTrabalhoReuniaoCommand command)
            => await Mediator.Send(command);

        #endregion

        #region Custos

        /// <summary>
        /// Obtém os custos de um plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/custos"), Produces("application/json", Type = typeof(IApplicationResult<PlanoTrabalhoCustoViewModel>))]
        public async Task<IActionResult> GetCustosById([FromRoute] Guid planoTrabalhoid)
            => await PlanoTrabalhoQuery.ObterCustosPlanoAsync(planoTrabalhoid);


        /// <summary>
        /// Adiciona um custo no plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{planoTrabalhoid}/custos"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostCustoPlano([FromRoute] Guid planoTrabalhoid, [FromBody] CadastrarPlanoTrabalhoCustoCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera um custo do plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="planoTrabalhoCustoId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{planoTrabalhoid}/custos/{planoTrabalhoCustoId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutCustoPlano([FromRoute] Guid planoTrabalhoid, [FromRoute] Guid planoTrabalhoCustoId, [FromBody] AlterarPlanoTrabalhoCustoCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            command.PlanoTrabalhoCustoId = planoTrabalhoCustoId;

            return await Mediator.Send(command);
        }


        /// <summary>
        /// Exclui um custo do plano
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{planoTrabalhoid}/custos/{planoTrabalhoCustoId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteCustoPlano([FromRoute] ExcluirPlanoTrabalhoCustoCommand command)
            => await Mediator.Send(command);

        #endregion

        #region Objetos

        /// <summary>
        /// Obtém os objetos de um plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/objetos"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PlanoTrabalhoObjetoViewModel>>))]
        public async Task<IActionResult> GetObjetosById([FromRoute] Guid planoTrabalhoid)
            => await PlanoTrabalhoQuery.ObterObjetosPlanoAsync(planoTrabalhoid);

        /// <summary>
        /// Obtém os objetos de um plano de trabalho e se estão associados à atividade do pacto de trabalho referente ao id passado como parâmetro
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="pactoTrabalhoAtividadeid"></param>
        /// <returns></returns>
        [HttpGet("{planoTrabalhoid}/objetosassociadosounaoatividadepacto"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PlanoTrabalhoObjetoViewModel>>))]
        public async Task<IActionResult> GetObjetosById([FromRoute] Guid planoTrabalhoid, [FromQuery] Guid pactoTrabalhoAtividadeid)
            => await PlanoTrabalhoQuery.ObterObjetosPlanoAssociadosOuNaoAAtividadesDoPactoAsync(planoTrabalhoid, pactoTrabalhoAtividadeid);

        /// <summary>
        /// Obtém um objeto de um plano de trabalho pelo id
        /// </summary>
        /// <param name="planoTrabalhoObjetoid"></param>
        /// <returns></returns>
        [HttpGet("objetos/{planoTrabalhoObjetoId}"), Produces("application/json", Type = typeof(IApplicationResult<PlanoTrabalhoObjetoViewModel>))]
        public async Task<IActionResult> GetObjetoById([FromRoute] Guid planoTrabalhoObjetoid)
            => await PlanoTrabalhoQuery.ObterObjetoPlanoByIdAsync(planoTrabalhoObjetoid);

        /// <summary>
        /// Exclui um objeto do plano
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{planoTrabalhoid}/objetos/{planoTrabalhoObjetoId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteObjetoPlano([FromRoute] ExcluirPlanoTrabalhoObjetoCommand command)
            => await Mediator.Send(command);

        /// <summary>
        /// Adiciona um objeto no plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{planoTrabalhoid}/objetos"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostObjetosPlano([FromRoute] Guid planoTrabalhoid, [FromBody] CadastrarPlanoTrabalhoObjetoCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera um objeto do plano de trabalho
        /// </summary>
        /// <param name="planoTrabalhoid"></param>
        /// <param name="planoTrabalhoObjetoId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{planoTrabalhoid}/objetos/{planoTrabalhoObjetoId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutObjetoPlano([FromRoute] Guid planoTrabalhoid, [FromRoute] Guid planoTrabalhoObjetoId, [FromBody] AlterarPlanoTrabalhoObjetoCommand command)
        {
            command.PlanoTrabalhoId = planoTrabalhoid;
            command.PlanoTrabalhoObjetoId = planoTrabalhoObjetoId;

            return await Mediator.Send(command);
        }

        #endregion

    }
}