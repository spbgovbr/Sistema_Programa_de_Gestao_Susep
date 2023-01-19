using Susep.SISRH.Application.Requests;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    public class AgendamentoFiltroRequest : UsuarioLogadoRequest
    {


        [DataMember(Name = "pessoaId")]
        public Int64? PessoaId { get; set; }

        [DataMember(Name = "dataInicio")]
        public DateTime? DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime? DataFim { get; set; }

    }
}
