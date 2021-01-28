using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "feriado")]
    public class FeriadoViewModel
    {

        [DataMember(Name = "data")]
        public DateTime Data { get; set; }

        [DataMember(Name = "fixo")]
        public bool Fixo { get; set; }

        [DataMember(Name = "ufId")]
        public String UfId { get; set; }
    }
}
