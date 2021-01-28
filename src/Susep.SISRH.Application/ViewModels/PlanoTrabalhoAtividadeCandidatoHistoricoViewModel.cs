using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoHistorico")]
    public class PlanoTrabalhoAtividadeCandidatoHistoricoViewModel
    {

        [DataMember(Name = "planoTrabalhoAtividadeCandidatoHistoricoId")]
        public Guid PlanoTrabalhoAtividadeCandidatoHistoricoId { get; set; }

        [DataMember(Name = "planoTrabalhoAtividadeCandidatoId")]
        public Guid PlanoTrabalhoAtividadeCandidatoId { get; set; }

        [DataMember(Name = "situacaoId")]
        public Int32 SituacaoId { get; set; }

        [DataMember(Name = "data")]
        public String data { get; set; }

        [DataMember(Name = "descricao")]
        public String descricao { get; set; }

        [DataMember(Name = "responsavelOperacao")]
        public String ResponsavelOperacao { get; set; }


    }


}
