using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "Assunto")]
    public class AssuntoViewModel
    {
        [DataMember(Name = "assuntoId")]
        public Guid AssuntoId { get; set; }

        [DataMember(Name = "valor")]
        public String Valor { get; set; }

        [DataMember(Name = "hierarquia")]
        public String Hierarquia { get; set; }

        [DataMember(Name = "nivel")]
        public int Nivel { get; set; }

        [DataMember(Name = "ativo")]
        public Boolean Ativo { get; set; }
    }

    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "AssuntoEdicao")]
    public class AssuntoEdicaoViewModel
    {
        [DataMember(Name = "assuntoId")]
        public Guid AssuntoId { get; set; }

        [DataMember(Name = "valor")]
        public String Valor { get; set; }

        [DataMember(Name = "assuntoPaiId")]
        public Guid? AssuntoPaiId { get; set; }

        [DataMember(Name = "pai")]
        public AssuntoEdicaoViewModel Pai { get; set; }

        [DataMember(Name = "ativo")]
        public Boolean Ativo { get; set; }
    }

}
