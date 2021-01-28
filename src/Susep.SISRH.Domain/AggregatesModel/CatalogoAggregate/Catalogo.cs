using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;
using SUSEP.Framework.SeedWorks.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate
{
    /// <summary>
    /// Representa os tipos de atividades com as quais uma determinada unidade pode atuar
    /// </summary>
    public class Catalogo : Entity
    {

        public Guid CatalogoId { get; private set; }
        public Int64 UnidadeId { get; private set; }

        public Unidade Unidade { get; private set; }

        public List<CatalogoItemCatalogo> ItensCatalogo { get; private set; }


        public static Catalogo Criar(Int64 unidadeId)
        {
            return new Catalogo()
            {
                CatalogoId = Guid.NewGuid(),
                UnidadeId = unidadeId                
            };
        }


        public void AdicionarItem(Guid itemCatalogoId)
        {
            this.ItensCatalogo.Add(CatalogoItemCatalogo.Criar(this.CatalogoId, itemCatalogoId));
        }

        public void RemoverItem(Guid itemCatalogoId)
        {
            this.ItensCatalogo.RemoveAll(i => i.ItemCatalogoId == itemCatalogoId);
        }


    }
}
