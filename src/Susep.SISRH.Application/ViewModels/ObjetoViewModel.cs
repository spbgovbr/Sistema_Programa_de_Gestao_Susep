using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "Objeto")]
    public class ObjetoViewModel
    {
        [DataMember(Name = "objetoId")]
        public Guid ObjetoId { get; set; }

        [DataMember(Name = "chave")]
        public String Chave { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

        [DataMember(Name = "tipo")]
        public String Tipo { get; set; }

        [DataMember(Name = "ativo")]
        public bool Ativo { get; set; }

    }

}
