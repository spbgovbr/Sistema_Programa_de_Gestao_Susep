using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate
{
    public interface IItemCatalogoRepository
    {
        Task<ItemCatalogo> ObterAsync(Guid itemCatalogoId);

        Task<ItemCatalogo> AdicionarAsync(ItemCatalogo item);

        void Atualizar(ItemCatalogo item);

        void Excluir(ItemCatalogo item);

        Task ExcluirAsync(Guid itemCatalogoId);
    }
}
