using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoHistorico")]
    public class PlanoTrabalhoHistoricoViewModel
    {

        [DataMember(Name = "planoTrabalhoHistoricoId")]
        public Guid PlanoTrabalhoHistoricoId { get; set; }

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }

        [DataMember(Name = "situacao")]
        public String Situacao { get; set; }

        [DataMember(Name = "observacoes")]
        public String Observacoes { get; set; }

        [DataMember(Name = "responsavelOperacao")]
        public String ResponsavelOperacao { get; set; }

        [DataMember(Name = "dataOperacao")]
        public String DataOperacao { get; set; }


    }


}
