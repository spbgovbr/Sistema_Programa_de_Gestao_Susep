using Microsoft.EntityFrameworkCore;
using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;
using Susep.SISRH.Infrastructure.Contexts;
using SUSEP.Framework.Data.Concrete.Repositories;
using System;
using System.Threading.Tasks;

namespace Susep.SISRH.Infrastructure.Repositories
{
    public class AssuntoRepository : SqlServerRepository<Assunto>, IAssuntoRepository
    {
        private readonly SISRHDbContext _context;

        public AssuntoRepository(SISRHDbContext context) : base(context) {
            _context = context;
        }

        public async Task<Assunto> ObterAsync(Guid id)
        {
            var item = await Entity.FindAsync(id);
            if (item != null)
            {
                await _context.Entry(item)
                    .Reference<Assunto>(i => i.AssuntoPai).LoadAsync();
            }
            return item;
        }

        public async Task<Assunto> AdicionarAsync(Assunto item)
        {
            var result = await Entity.AddAsync(item);
            return result.Entity;
        }

        public void Atualizar(Assunto item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Excluir(Assunto item)
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
