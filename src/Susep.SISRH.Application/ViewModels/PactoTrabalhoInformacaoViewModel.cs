using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "pactoTrabalhoInformacao")]
    public class PactoTrabalhoInformacaoViewModel
    {

        [DataMember(Name = "pactoTrabalhoInformacaoId")]
        public Guid PactoTrabalhoInformacaoId { get; set; }

        [DataMember(Name = "pactoTrabalhoId")]
        public Guid PactoTrabalhoId { get; set; }

        [DataMember(Name = "dataRegistro")]
        public DateTime DataRegistro { get; set; }

        [DataMember(Name = "responsavelRegistro")]
        public String ResponsavelRegistro { get; set; }

        [DataMember(Name = "informacao")]
        public String Informacao { get; set; }

    }


}
