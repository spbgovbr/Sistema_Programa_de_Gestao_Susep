using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Commands.PactoTrabalho;
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
    /// Operações com os pactos de trabalho
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    [ApiController]
    public class PactoTrabalhoController : ControllerBase
    {
        private IMediator Mediator { get; }
        private readonly IPactoTrabalhoQuery PactoTrabalhoQuery;
        private readonly IEstruturaOrganizacionalQuery EstruturaOrganizacionalQuery;

        /// <summary>
        /// Contrutor da classe
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="pactoTrabalhoQuery"></param>
        /// <param name="estruturaOrganizacionalQuery"></param>
        public PactoTrabalhoController(IMediator mediator, 
            IPactoTrabalhoQuery pactoTrabalhoQuery,
            IEstruturaOrganizacionalQuery estruturaOrganizacionalQuery)
        {
            Mediator = mediator;
            PactoTrabalhoQuery = pactoTrabalhoQuery;
            EstruturaOrganizacionalQuery = estruturaOrganizacionalQuery;
        }

        /// <summary>
        /// Obtém os pactos de trabalho cadastrados
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet(), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PactoTrabalhoViewModel>>))]
        public async Task<IActionResult> GetAll([FromQuery]PactoTrabalhoFiltroRequest request)
            => await EstruturaOrganizacionalQuery.ObterPactoTrabalhoPorFiltroAsync(request);

        /// <summary>
        /// Obtém o pacto de trabalho atual do usuário
        /// </summary>
        /// <returns></returns>
        [HttpGet("atual"), Produces("application/json", Type = typeof(IApplicationResult<PactoTrabalhoViewModel>))]
        public async Task<IActionResult> GetById([FromQuery]UsuarioLogadoRequest request)
            => await PactoTrabalhoQuery.ObterAtualAsync(request);

        /// <summary>
        /// Obtém os dados de um pacto de trabalho
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{pactoTrabalhoid}"), Produces("application/json", Type = typeof(IApplicationResult<PactoTrabalhoViewModel>))]
        public async Task<IActionResult> GetById([FromRoute]PactoTrabalhoRequest request)
            => await EstruturaOrganizacionalQuery.ObterPactoTrabalhoPorChaveAsync(request);


        /// <summary>
        /// Cadastra um pacto de trabalho do corretor logado
        /// </summary>
        /// <param name="command">Dados do pacto de trabalho a ser cadastrado</param>
        /// <returns></returns>
        [HttpPost(), Produces("application/json", Type = typeof(IApplicationResult<Guid>))]
        public async Task<IActionResult> PostPactoTrabalho([FromBody] CadastrarPactoTrabalhoCommand command)
            => await Mediator.Send(command);


        /// <summary>
        /// Altera os dados de um pacto de trabalho do corretor logado
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutPactoTrabalho([FromRoute] Guid pactoTrabalhoid, [FromBody] AlterarPactoTrabalhoCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera os dados de um pacto de trabalho do corretor logado
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/periodo"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutPactoTrabalhoDatas([FromRoute] Guid pactoTrabalhoid, [FromBody] AlterarPeriodoPactoTrabalhoCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera os dados de um pacto de trabalho do corretor logado
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <returns></returns>
        [HttpDelete("{pactoTrabalhoid}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeletePactoTrabalho([FromRoute] Guid pactoTrabalhoid)
        {
            var command = new ExcluirPactoTrabalhoCommand() { PactoTrabalhoId = pactoTrabalhoid };            
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Obtém o histórico de um pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{pactoTrabalhoid}/historico"), Produces("application/json", Type = typeof(IApplicationResult<IEnumerable<PactoTrabalhoHistoricoViewModel>>))]
        public async Task<IActionResult> GetHistoricoById([FromRoute]Guid pactoTrabalhoid)
            => await PactoTrabalhoQuery.ObterHistoricoPorPactoAsync(pactoTrabalhoid);


        #region Fluxo

        /// <summary>
        /// Altera a situação do pacto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/enviarParaAceite"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> EnviarParaAprovacao([FromRoute]AlterarSituacaoPactoTrabalhoCommand command)
        {
            command.SituacaoId = (int)SituacaoPactoTrabalhoEnum.EnviadoAceite;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera a situação do pacto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/aceitar"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> Aprovar([FromRoute]AlterarSituacaoPactoTrabalhoCommand command)
        {
            command.SituacaoId = (int)SituacaoPactoTrabalhoEnum.Aceito;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera a situação do pacto
        /// </summary>
        /// <param name="command"></param>
        /// <param name="pactoTrabalhoid"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/rejeitar"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> Rejeitar([FromRoute]Guid pactoTrabalhoid, [FromBody]AlterarSituacaoPactoTrabalhoCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            command.SituacaoId = (int)SituacaoPactoTrabalhoEnum.Rejeitado;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera a situação do pacto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/voltarParaRascunho"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> VoltarParaRascunho([FromRoute]AlterarSituacaoPactoTrabalhoCommand command)
        {
            command.SituacaoId = (int)SituacaoPactoTrabalhoEnum.Rascunho;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera a situação do pacto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/iniciarExecucao"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> IniciarExecucao([FromRoute]AlterarSituacaoPactoTrabalhoCommand command)
        {
            command.SituacaoId = (int)SituacaoPactoTrabalhoEnum.EmExecucao;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera a situação do pacto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/concluirExecucao"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> ConcluirExecucao([FromRoute]AlterarSituacaoPactoTrabalhoCommand command)
        {
            command.SituacaoId = (int)SituacaoPactoTrabalhoEnum.Executado;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera a situação do pacto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/reabrir"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> Reabrir([FromRoute] AlterarSituacaoPactoTrabalhoCommand command)
        {
            command.SituacaoId = (int)SituacaoPactoTrabalhoEnum.EmExecucao;
            return await Mediator.Send(command);
        }
        

        /// <summary>
        /// Altera a situação do pacto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/avaliar"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> Avaliar([FromRoute]AlterarSituacaoPactoTrabalhoCommand command)
        {
            command.SituacaoId = (int)SituacaoPactoTrabalhoEnum.Concluido;
            return await Mediator.Send(command);
        }

        #endregion

        #region Atividades

        /// <summary>
        /// Obtém as atividades de um pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{pactoTrabalhoid}/atividades"), Produces("application/json", Type = typeof(IApplicationResult<PactoTrabalhoAtividadeViewModel>))]
        public async Task<IActionResult> GetAtividadesById([FromRoute]Guid pactoTrabalhoid)
            => await PactoTrabalhoQuery.ObterAtividadesPactoAsync(pactoTrabalhoid);
        

        /// <summary>
        /// Adiciona uma atividade no pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{pactoTrabalhoid}/atividades"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostAtividadePacto([FromRoute]Guid pactoTrabalhoid, [FromBody]CadastrarPactoTrabalhoAtividadeCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera uma atividade do pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="pactoTrabalhoAtividadeId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/atividades/{pactoTrabalhoAtividadeId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutAtividadePacto([FromRoute]Guid pactoTrabalhoid, [FromRoute]Guid pactoTrabalhoAtividadeId, [FromBody]AlterarPactoTrabalhoAtividadeCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            command.PactoTrabalhoAtividadeId = pactoTrabalhoAtividadeId;

            return await Mediator.Send(command);
        }

        /// <summary>
        /// Exclui uma atividade do pacto
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete("{pactoTrabalhoid}/atividades/{pactoTrabalhoAtividadeId}"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> DeleteAtividadePacto([FromRoute]ExcluirPactoTrabalhoAtividadeCommand command)
            => await Mediator.Send(command);



        /// <summary>
        /// Altera uma atividade do pacto de trabalho
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/atividades/{pactoTrabalhoAtividadeId}/todo"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutAtividadePactoTodo([FromRoute]AlterarSituacaoPactoTrabalhoAtividadeCommand command)
        {
            command.SituacaoId = (int)SituacaoAtividadePactoTrabalhoEnum.ToDo;
            return await Mediator.Send(command);
        }


        /// <summary>
        /// Altera uma atividade do pacto de trabalho
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/atividades/{pactoTrabalhoAtividadeId}/doing"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutAtividadePactoDoing([FromRoute]AlterarSituacaoPactoTrabalhoAtividadeCommand command)
        {
            command.SituacaoId = (int)SituacaoAtividadePactoTrabalhoEnum.Doing;
            return await Mediator.Send(command);
        }


        /// <summary>
        /// Altera uma atividade do pacto de trabalho
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/atividades/{pactoTrabalhoAtividadeId}/done"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutAtividadePactoDone([FromRoute]AlterarSituacaoPactoTrabalhoAtividadeCommand command)
        {
            command.SituacaoId = (int)SituacaoAtividadePactoTrabalhoEnum.Done;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera uma atividade do pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="pactoTrabalhoAtividadeId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/atividades/{pactoTrabalhoAtividadeId}/andamento"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutAtividadePactoAndamento([FromRoute]Guid pactoTrabalhoid, [FromRoute]Guid pactoTrabalhoAtividadeId, [FromBody]AlterarAndamentoPactoTrabalhoAtividadeCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            command.PactoTrabalhoAtividadeId = pactoTrabalhoAtividadeId;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Avaliar uma atividade do pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="pactoTrabalhoAtividadeId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/atividades/{pactoTrabalhoAtividadeId}/avaliar"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutAvaliarAtividade([FromRoute] Guid pactoTrabalhoid, [FromRoute] Guid pactoTrabalhoAtividadeId, [FromBody] AvaliarAtividadePactoTrabalhoCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            command.PactoTrabalhoAtividadeId = pactoTrabalhoAtividadeId;
            return await Mediator.Send(command);
        }


        #endregion

        #region Solicitações

        /// <summary>
        /// Obtém as solicitações de um pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{pactoTrabalhoid}/solicitacao"), Produces("application/json", Type = typeof(IApplicationResult<PactoTrabalhoSolicitacaoViewModel>))]
        public async Task<IActionResult> GetSolicitacoesById([FromRoute]Guid pactoTrabalhoid)
            => await PactoTrabalhoQuery.ObteSolicitacoesPactoAsync(pactoTrabalhoid);


        /// <summary>
        /// Adiciona uma atividade no pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{pactoTrabalhoid}/solicitacao/prazo"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostProporAlterarPrazoPacto([FromRoute]Guid pactoTrabalhoid, [FromBody]ProporPactoTrabalhoPrazoCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            return await Mediator.Send(command);
        }


        /// <summary>
        /// Adiciona uma atividade no pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{pactoTrabalhoid}/solicitacao/atividade"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostProporAtividadePacto([FromRoute]Guid pactoTrabalhoid, [FromBody]ProporPactoTrabalhoAtividadeCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Justificativa de estouro de prazo
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{pactoTrabalhoid}/solicitacao/justificarestouroprazo"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostJustificarEstouroPrazoAtividade([FromRoute] Guid pactoTrabalhoid, [FromBody] JustificarEstouroPrazoAtividadeCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            return await Mediator.Send(command);
        }


        /// <summary>
        /// Solicita a exclusão de uma atividade do pacto
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("{pactoTrabalhoid}/solicitacao/excluiratividade"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PostProporExcluirAtividadePacto([FromRoute] Guid pactoTrabalhoid, [FromBody] ProporExcluirAtividadeCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            return await Mediator.Send(command);
        }

        /// <summary>
        /// Altera uma atividade do pacto de trabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <param name="pactoTrabalhoSolicitacaoId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{pactoTrabalhoid}/solicitacao/{pactoTrabalhoSolicitacaoId}/responder"), Produces("application/json", Type = typeof(IApplicationResult<bool>))]
        public async Task<IActionResult> PutResponderPropostaAtividadePacto([FromRoute]Guid pactoTrabalhoid, [FromRoute]Guid pactoTrabalhoSolicitacaoId, [FromBody]ResponderSolitacaoPactoTrabalhoAtividadeCommand command)
        {
            command.PactoTrabalhoId = pactoTrabalhoid;
            command.PactoTrabalhoSolicitacaoId = pactoTrabalhoSolicitacaoId;
            return await Mediator.Send(command);
        }

        #endregion

        #region Assuntos

        /// <summary>
        /// Obtém os assuntos para associar às atividades de um PactoTrabalho
        /// </summary>
        /// <param name="pactoTrabalhoid"></param>
        /// <returns></returns>
        [HttpGet("{pactoTrabalhoid}/assuntosParaAssociar"), Produces("application/json", Type = typeof(IApplicationResult<PactoTrabalhoAssuntosParaAssociarViewModel>))]
        public async Task<IActionResult> GetAssuntosParaAssociar([FromRoute] Guid pactoTrabalhoid)
            => await PactoTrabalhoQuery.ObterAssuntosParaAssociarAsync(pactoTrabalhoid);


        #endregion

    }
}