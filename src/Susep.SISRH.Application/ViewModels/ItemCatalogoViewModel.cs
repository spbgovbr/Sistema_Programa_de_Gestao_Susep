using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Susep.SISRH.Application.ViewModels
{
    [DataContract(Namespace = "http://www.susep.gov.br/sisrh/viewmodels/", Name = "itemCatalogo")]
    public class ItemCatalogoViewModel
    {

        [DataMember(Name = "itemCatalogoId")]
        public Guid ItemCatalogoId { get; set; }

        [DataMember(Name = "titulo")]
        public String Titulo { get; set; }

        [DataMember(Name = "tituloCompleto")]
        public String TituloCompleto { get; set; }
        

        [DataMember(Name = "descricao")]
        public String Descricao { get; set; }

        [DataMember(Name = "formaCalculoTempoItemCatalogoId")]
        public Int32 FormaCalculoTempoItemCatalogoId { get; set; }

        [DataMember(Name = "formaCalculoTempoItemCatalogo")]
        public String FormaCalculoTempoItemCatalogo { get; set; }

        [DataMember(Name = "permiteTrabalhoRemoto")]
        public Boolean PermiteTrabalhoRemoto { get; set; }

        [DataMember(Name = "tempoExecucaoPresencial")]
        public Decimal? TempoExecucaoPresencial { get; set; }

        [DataMember(Name = "tempoExecucaoRemoto")]
        public Decimal? TempoExecucaoRemoto { get; set; }

        [DataMember(Name = "complexidade")]
        public String Complexidade { get; private set; }

        [DataMember(Name = "definicaoComplexidade")]
        public String DefinicaoComplexidade { get; private set; }

        [DataMember(Name = "entregasEsperadas")]
        public String EntregasEsperadas { get; private set; }

        [DataMember(Name = "temPactoCadastrado")]
        public bool TemPactoCadastrado { get; set; }

        [DataMember(Name = "temUnidadeAssociada")]
        public bool TemUnidadeAssociada { get; set; }

        


        [DataMember(Name ="assuntos")]
        public IEnumerable<AssuntoViewModel> Assuntos { get; set; }

    }
}
