using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate
{
    /// <summary>
    /// Representa os tipos de atividades com as quais uma determinada unidade pode atuar
    /// </summary>
    public class CatalogoItemCatalogo : Entity
    {

        public Guid CatalogoItemCatalogoId { get; private set; }
        public Guid CatalogoId { get; private set; }
        public Guid ItemCatalogoId { get; private set; }

        public Catalogo Catalogo { get; private set; }
        public ItemCatalogo ItemCatalogo { get; private set; }


        public static CatalogoItemCatalogo Criar(Guid catalogoId, Guid itemCatalogoId)
        {
            return new CatalogoItemCatalogo()
            {
                //CatalogoItemCatalogoId = Guid.NewGuid(),
                CatalogoId = catalogoId,
                ItemCatalogoId = itemCatalogoId
            };
        }

    }
}
