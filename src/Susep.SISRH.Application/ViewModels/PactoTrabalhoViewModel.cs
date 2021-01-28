using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PactoTrabalho")]
    public class PactoTrabalhoViewModel
    {

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }        

        [DataMember(Name = "unidadeId")]
        public Int64 UnidadeId { get; set; }

        [DataMember(Name = "unidade")]
        public String Unidade { get; set; }

        [DataMember(Name = "pessoaId")]
        public Int64 PessoaId { get; set; }

        [DataMember(Name = "pessoa")]
        public String Pessoa { get; set; }

        [DataMember(Name = "dataInicio")]
        public DateTime DataInicio { get; set; }

        [DataMember(Name = "dataFim")]
        public DateTime DataFim { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }

        [DataMember(Name = "situacao")]
        public String Situacao { get; set; }

        [DataMember(Name = "formaExecucaoId")]
        public Int32 FormaExecucaoId { get; set; }

        [DataMember(Name = "formaExecucao")]
        public String FormaExecucao { get; set; }

        [DataMember(Name = "tempoTotalDisponivel")]
        public Int32 TempoTotalDisponivel { get; set; }

        [DataMember(Name = "cargaHorariaDiaria")]
        public String CargaHorariaDiaria { get; set; }

        [DataMember(Name = "percentualExecucao")]
        public String PercentualExecucao { get; set; }

        [DataMember(Name = "relacaoPrevistoRealizado")]
        public String RelacaoPrevistoRealizado { get; set; }

        [DataMember(Name = "tempoComparecimento")]
        public String TempoComparecimento { get; set; }

        [DataMember(Name = "responsavelEnvioAceite")]
        public String ResponsavelEnvioAceite { get; set; }
        
    }
}
