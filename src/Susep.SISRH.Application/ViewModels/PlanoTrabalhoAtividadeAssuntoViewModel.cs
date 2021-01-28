using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoAtividadeCriterio")]
    public class PlanoTrabalhoAtividadeAssuntoViewModel
    {
        [DataMember(Name = "planoTrabalhoAtividadeAssuntoId")]
        public Guid PlanoTrabalhoAtividadeAssuntoId { get; set; }

        [DataMember(Name = "planoTrabalhoAtividadeId")]
        public Guid PlanoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "assuntoId")]
        public Guid AssuntoId { get; set; }

    }


}
