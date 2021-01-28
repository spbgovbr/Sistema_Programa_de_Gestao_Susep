using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "PlanoTrabalhoAtividade")]
    public class PlanoTrabalhoAtividadeItemViewModel
    {
        [DataMember(Name = "planoTrabalhoAtividadeItemId")]
        public Guid PlanoTrabalhoAtividadeItemId { get; set; }

        [DataMember(Name = "planoTrabalhoAtividadeId")]
        public Guid PlanoTrabalhoAtividadeId { get; set; }

        [DataMember(Name = "itemCatalogoId")]
        public Guid ItemCatalogoId { get; set; }

        [DataMember(Name = "itemCatalogo")]
        public String ItemCatalogo { get; set; }

        [DataMember(Name = "itemCatalogoComplexidade")]
        public String ItemCatalogoComplexidade { get; set; }

    }


}
