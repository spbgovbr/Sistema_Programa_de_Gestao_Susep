using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "Catalogo")]
    public class CatalogoViewModel
    {

        [DataMember(Name = "catalogoId")]
        public Guid CatalogoId { get; set; }

        [DataMember(Name = "unidadeId")]
        public Int64 UnidadeId { get; set; }

        [DataMember(Name = "unidadeSigla")]
        public String UnidadeSigla { get; set; }

        [DataMember(Name = "itens")]
        public IEnumerable<ItemCatalogoViewModel> Itens { get; set; }

        [DataMember(Name = "unidade")]
        public UnidadeViewModel Unidade { get; set; }

    }
}
