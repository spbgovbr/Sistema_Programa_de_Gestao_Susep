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
    public class CadastrarAgendamentoCommandHandler : IRequestHandler<CadastrarAgendamentoCommand, IActionResult>
    {     
        private IAgendamentoRepository AgendamentoRepository { get; }
        private IUnitOfWork UnitOfWork { get; }

        public CadastrarAgendamentoCommandHandler(
            IAgendamentoRepository agendamentoRepository, 
            IUnitOfWork unitOfWork)
        {
            AgendamentoRepository = agendamentoRepository;
            UnitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Handle(CadastrarAgendamentoCommand request, CancellationToken cancellationToken)
        {
            ApplicationResult<bool> result = new ApplicationResult<bool>(request);

            //Monta o objeto com os dados do agendamento
            var agendamento = Domain.AggregatesModel.AgendamentoAggregate.Agendamento.Criar(Int64.Parse(request.UsuarioLogadoId.ToString()), request.Data);

            //Adiciona o agendamento no banco de dados
            await AgendamentoRepository.AdicionarAsync(agendamento);
            UnitOfWork.Commit(false);

            result.Result = true;
            result.SetHttpStatusToOk("Agendamento cadastrado com sucesso.");
            return result;
        }
    }
}
