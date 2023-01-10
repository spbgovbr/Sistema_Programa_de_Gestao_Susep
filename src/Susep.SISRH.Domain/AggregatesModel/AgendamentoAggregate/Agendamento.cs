using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;

namespace Susep.SISRH.Domain.AggregatesModel.AgendamentoAggregate
{
    /// <summary>
    /// Representa os Agendamentos
    /// </summary>
    public class Agendamento : Entity
    {
        public Guid AgendamentoId { get; private set; }
        public Int64 PessoaId { get; private set; }
        public DateTime DataAgendada { get; private set; }
        public Pessoa Pessoa { get; private set; }


        public static Agendamento Criar(Int64 pessoaId, DateTime dataAgendada)
        {
            return new Agendamento()
            {
                //AgendamentoId = Guid.NewGuid(),
                PessoaId = pessoaId,
                DataAgendada = dataAgendada
            };
        }

    }
}
