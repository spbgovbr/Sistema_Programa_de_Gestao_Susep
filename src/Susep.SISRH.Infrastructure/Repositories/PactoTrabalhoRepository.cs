using Microsoft.EntityFrameworkCore;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;
using Susep.SISRH.Infrastructure.Contexts;
using SUSEP.Framework.Data.Concrete.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Susep.SISRH.Infrastructure.Repositories
{
    public class PactoTrabalhoRepository : SqlServerRepository<PactoTrabalho>, IPactoTrabalhoRepository
    {
        private readonly SISRHDbContext _context;

        public PactoTrabalhoRepository(SISRHDbContext context) : base(context) {
            _context = context;
        }

        public async Task<PactoTrabalho> ObterAsync(Guid id)
        {
            return await Entity
                .Include(p => p.PlanoTrabalho)
                .Include(p => p.Atividades)
                    .ThenInclude(a => a.ItemCatalogo)
                .Include(p => p.Atividades)
                    .ThenInclude(a => a.Assuntos)
                .Include(p => p.Atividades)
                    .ThenInclude(a => a.Objetos)
                .Include(p => p.Solicitacoes)
                .Include(p => p.Historico)
                .Include(p => p.Pessoa)
                .FirstOrDefaultAsync(p => p.PactoTrabalhoId == id);
        }

        public async Task<PactoTrabalho> AdicionarAsync(PactoTrabalho item)
        {
            var result = await Entity.AddAsync(item);
            return result.Entity;
        }

        public void Atualizar(PactoTrabalho item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Excluir(PactoTrabalho item)
        {
            _context.Entry(item).State = EntityState.Deleted;
        }
    }
}
