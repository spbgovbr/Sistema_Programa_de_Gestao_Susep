using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoAtividadeCriterio")]
    public class PlanoTrabalhoAtividadeCriterioViewModel
    {
        [DataMember(Name = "planoTrabalhoAtividadeCriterioId")]
        public Guid PlanoTrabalhoAtividadeItemId { get; set; }

        [DataMember(Name = "planoTrabalhoAtividadeId")]
        public Guid PlanoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "criterioId")]
        public Int32 CriterioId { get; set; }

        [DataMember(Name = "criterio")]
        public String Criterio { get; set; }

    }


}
