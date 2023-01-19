using Microsoft.EntityFrameworkCore;
using Susep.SISRH.Domain.AggregatesModel.AgendamentoAggregate;
using Susep.SISRH.Infrastructure.Contexts;
using SUSEP.Framework.Data.Concrete.Repositories;
using System;
using System.Threading.Tasks;

namespace Susep.SISRH.Infrastructure.Repositories
{
    public class AgendamentoRepository : SqlServerRepository<Agendamento>, IAgendamentoRepository
    {
        private readonly SISRHDbContext _context;

        public AgendamentoRepository(SISRHDbContext context) : base(context) {
            _context = context;
        }

        public async Task<Agendamento> ObterAsync(Guid id)
        {
            var item = await Entity.FindAsync(id);
            return item;
        }

        public async Task<Agendamento> AdicionarAsync(Agendamento item)
        {
            var result = await Entity.AddAsync(item);
            return result.Entity;
        }

        public void Atualizar(Agendamento item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Excluir(Agendamento item)
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
