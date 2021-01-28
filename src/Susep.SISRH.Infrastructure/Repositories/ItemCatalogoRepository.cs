using Microsoft.EntityFrameworkCore;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Infrastructure.Contexts;
using SUSEP.Framework.Data.Concrete.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Susep.SISRH.Infrastructure.Repositories
{
    public class ItemCatalogoRepository : SqlServerRepository<ItemCatalogo>, IItemCatalogoRepository
    {
        private readonly SISRHDbContext _context;

        public ItemCatalogoRepository(SISRHDbContext context) : base(context) {
            _context = context;
        }

        public async Task<ItemCatalogo> ObterAsync(Guid id)
        {
            var item = await Entity.FindAsync(id);
            if (item != null)
            {
                //await _context.Entry(item)
                //    .Collection(i => i.Prepostos).LoadAsync();                
                await _context.Entry(item)
                    .Collection(i => i.Assuntos).LoadAsync();
            }
            return item;
        }

        public async Task<ItemCatalogo> AdicionarAsync(ItemCatalogo item)
        {
            var result = await Entity.AddAsync(item);
            return result.Entity;
        }

        public void Atualizar(ItemCatalogo item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Excluir(ItemCatalogo item)
        {
            _context.Entry(item).State = EntityState.Deleted;
        }

        public async Task ExcluirAsync(Guid id)
        {
            var item = await Entity.FindAsync(id);
            _context.Entry(item).State = EntityState.Deleted;
        }

    }
}
