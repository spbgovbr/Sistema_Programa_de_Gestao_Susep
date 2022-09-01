using MediatR;
using Microsoft.AspNetCore.Mvc;
using Susep.SISRH.Application.Queries.Abstractions;
using Susep.SISRH.Domain.AggregatesModel.AgendamentoAggregate;
using SUSEP.Framework.Data.Abstractions.UnitOfWorks;
using SUSEP.Framework.Result.Concrete;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Susep.SISRH.Application.Commands.Agendamento
{
    public class ExcluirAgendamentoCommandHandler : IRequestHandler<ExcluirAgendamentoCommand, IActionResult>
    {
        private IAgendamentoRepository AgendamentoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public ExcluirAgendamentoCommandHandler(
            IAgendamentoRepository agendamentoRepository,
            IUnitOfWork unitOfWork)
        {
            AgendamentoRepository = agendamentoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(ExcluirAgendamentoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>();

            try
            {
                //Monta o objeto com os dados do agendamento
                var agendamento = await AgendamentoRepository.ObterAsync(request.AgendamentoId);

                //if (agendamento.PessoaId != Int64.Parse(request.UsuarioLogadoId.ToString()))
                //{
                //    result.Result = true;
                //    result.SetHttpStatusToBadRequest("Não é possível excluir um agendamento de outra pessoa.");
                //    return result;
                //}

                if (agendamento.DataAgendada.Date <= DateTime.Now.Date)
                {
                    result.Result = true;
                    result.SetHttpStatusToBadRequest("Não é possível excluir um agendamento de data anterior ou igual à atual.");
                    return result;
                }

                //Adiciona o agendamento no banco de dados
                AgendamentoRepository.Excluir(agendamento);
                UnitOfWork.Commit(false);
            }
            catch (System.Exception ex)
            {

            }

            result.Result = true;
            result.SetHttpStatusToOk("Agendamento excluído com sucesso.");
            return result;
        }
    }
}
