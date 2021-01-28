using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalho")]
    public class PlanoTrabalhoViewModel
    {

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "unidadeId")]
        public Int64 UnidadeId { get; set; }

        [DataMember(Name = "unidade")]
        public String Unidade { get; set; }

        [DataMember(Name = "dataInicio")]
        public DateTime DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime DataFim { get; set; }

        [DataMember(Name = "tempoComparecimento")]
        public Int32 TempoComparecimento { get; set; }

        [DataMember(Name = "tempoFaseHabilitacao")]
        public Int32 TempoFaseHabilitacao { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }

        [DataMember(Name = "situacao")]
        public String Situacao { get; set; }

        [DataMember(Name = "totalServidoresSetor")]
        public Int32 TotalServidoresSetor { get; set; }

        [DataMember(Name = "termoAceite")]
        public string TermoAceite { get; set; }

    }
}
