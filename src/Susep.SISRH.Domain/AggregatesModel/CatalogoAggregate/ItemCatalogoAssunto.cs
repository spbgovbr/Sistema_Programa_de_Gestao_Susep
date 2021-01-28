using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate
{
    /// <summary>
    /// Representa o relacionamento entre ItemCatalogo e Assunto
    /// </summary>
    public class ItemCatalogoAssunto : Entity
    {

        public Guid ItemCatalogoAssuntoId { get; private set; }
        public Guid ItemCatalogoId { get; private set; }
        public Guid AssuntoId { get; private set; }

        public Assunto Assunto { get; private set; }


        public static ItemCatalogoAssunto Criar(Guid itemCatalogoId, Guid assuntoId)
        {
            return new ItemCatalogoAssunto()
            {
                ItemCatalogoId = itemCatalogoId,
                AssuntoId = assuntoId
            };
        }

    }
}
