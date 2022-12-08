using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "agendamentoPresencial")]
    public class AgendamentoPresencialViewModel
    {

        [DataMember(Name = "agendamentoPresencialId")]
        public Guid AgendamentoPresencialId { get; set; }

        [DataMember(Name = "pessoaId")]
        public Int64 PessoaId { get; set; }

        [DataMember(Name = "pessoa")]
        public String Pessoa { get; set; }

        [DataMember(Name = "dataAgendada")]
        public DateTime DataAgendada { get; set; }

    }
}
