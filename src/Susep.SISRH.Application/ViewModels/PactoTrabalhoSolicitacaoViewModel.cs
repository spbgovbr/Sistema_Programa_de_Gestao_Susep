using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PactoTrabalhoSolicitacao")]
    public class PactoTrabalhoSolicitacaoViewModel
    {

        [DataMember(Name = "pactoTrabalhoSolicitacaoId")]
        public Guid PactoTrabalhoSolicitacaoId { get; set; }

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "tipoSolicitacaoId")]
        public Int32 TipoSolicitacaoId { get; set; }

        [DataMember(Name = "tipoSolicitacao")]
        public String TipoSolicitacao { get; set; }

        [DataMember(Name = "dataSolicitacao")]
        public DateTime DataSolicitacao { get; set; }

        [DataMember(Name = "solicitanteId")]
        public Int64 SolicitanteId { get; set; }

        [DataMember(Name = "solicitante")]
        public String Solicitante { get; set; }

        [DataMember(Name = "unidade")]
        public String Unidade { get; set; }

        [DataMember(Name = "dadosSolicitacao")]
        public String DadosSolicitacao { get; set; }

        [DataMember(Name = "observacoesSolicitante")]
        public String ObservacoesSolicitante { get; set; }

        [DataMember(Name = "analisado")]
        public Boolean Analisado { get; set; }

        [DataMember(Name = "dataAnalise")]
        public DateTime? DataAnalise { get; set; }

        [DataMember(Name = "analista")]
        public String Analista { get; set; }

        [DataMember(Name = "aprovado")]
        public Boolean? Aprovado { get; set; }

        [DataMember(Name = "observacoesAnalista")]
        public String ObservacoesAnalista { get; set; }

    }


}
