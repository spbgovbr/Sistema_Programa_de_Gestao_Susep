using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoMeta")]
    public class PlanoTrabalhoMetaViewModel
    {

        [DataMember(Name = "planoTrabalhoMetaId")]
        public Guid PlanoTrabalhoMetaId { get; set; }

        [DataMember(Name = "planoTrabalhoId")]
        public Guid PlanoTrabalhoId { get; set; }

        [DataMember(Name = "meta")]
        public String Meta { get; set; }

        [DataMember(Name = "indicador")]
        public String Indicador { get; set; }

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

    }
}
