using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoReuniao")]
    public class PlanoTrabalhoReuniaoViewModel
    {

        [DataMember(Name = "planoTrabalhoReuniaoId")]
        public Guid? PlanoTrabalhoReuniaoId { get; set; }

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "data")]
        public DateTime Data { get; set; }

        [DataMember(Name = "titulo")]
        public String Titulo { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

    }
}
