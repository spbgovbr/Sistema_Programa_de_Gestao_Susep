using Microsoft.EntityFrameworkCore;
using Susep.SISRH.Domain.AggregatesModel.ObjetoAggregate;
using Susep.SISRH.Infrastructure.Contexts;
using SUSEP.Framework.Data.Concrete.Repositories;
using System;
using System.Threading.Tasks;

namespace Susep.SISRH.Infrastructure.Repositories
{
    public class ObjetoRepository : SqlServerRepository<Objeto>, IObjetoRepository
    {
        private readonly SISRHDbContext _context;

        public ObjetoRepository(SISRHDbContext context) : base(context) {
            _context = context;
        }

        public async Task<Objeto> ObterAsync(Guid id)
        {
            var item = await Entity.FindAsync(id);
            return item;
        }

        public async Task<Objeto> AdicionarAsync(Objeto item)
        {
            var result = await Entity.AddAsync(item);
            return result.Entity;
        }

        public void Atualizar(Objeto item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Excluir(Objeto item)
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
