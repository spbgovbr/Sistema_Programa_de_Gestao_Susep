using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Domain.AggregatesModel.AgendamentoAggregate
{
    public interface IAgendamentoRepository
    {
        Task<Agendamento> ObterAsync(Guid AgendamentoId);
        Task<Agendamento> AdicionarAsync(Agendamento item);
        void Excluir(Agendamento item);

    }
}
