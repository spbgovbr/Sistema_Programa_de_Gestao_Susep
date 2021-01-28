using System;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "dadoscombo")]
    public class DadosComboViewModel
    {

        [DataMember(Name = "id")]
        public String Id { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

    }

}
