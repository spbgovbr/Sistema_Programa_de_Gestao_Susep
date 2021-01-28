using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoCusto")]
    public class PlanoTrabalhoCustoViewModel
    {

        [DataMember(Name = "planoTrabalhoCustoId")]
        public Guid? PlanoTrabalhoCustoId { get; set; }

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "valor")]
        public Decimal Valor { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

    }
}
