using Microsoft.EntityFrameworkCore;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;
using Susep.SISRH.Infrastructure.Contexts;
using SUSEP.Framework.Data.Concrete.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Susep.SISRH.Infrastructure.Repositories
{
    public class PlanoTrabalhoRepository : SqlServerRepository<PlanoTrabalho>, IPlanoTrabalhoRepository
    {
        private readonly SISRHDbContext _context;

        public PlanoTrabalhoRepository(SISRHDbContext context) : base(context) {
            _context = context;
        }

        public async Task<PlanoTrabalho> ObterAsync(Guid id)
        {
            return await Entity
                .Include(p => p.Atividades)
                    .ThenInclude(a => a.ItensCatalogo)
                .Include(p => p.Atividades)
                    .ThenInclude(a => a.Criterios)
                .Include(p => p.Atividades)
                    .ThenInclude(a => a.Candidatos)
                        .ThenInclude(a => a.Historico)
                .Include(p => p.Atividades)
                    .ThenInclude(a => a.Candidatos)
                        .ThenInclude(c => c.Pessoa)  
                .Include(p => p.Atividades)
                    .ThenInclude(a => a.Assuntos)
                .Include(p => p.PactosTrabalho)
                .Include(p => p.Unidade)
                .Include(p => p.Metas)
                .Include(p => p.Reunioes)
                .Include(p => p.Custos)
                .Include(p => p.Objetos)
                    .ThenInclude(p => p.Custos)
                .Include(p => p.Objetos)
                    .ThenInclude(p => p.Reunioes)
                .Include(p => p.Objetos)
                    .ThenInclude(p => p.Assuntos)
                        .ThenInclude(p => p.Assunto)
                .Include(p => p.Historico)
                .FirstOrDefaultAsync(p => p.PlanoTrabalhoId == id);
        }

        public async Task<PlanoTrabalho> AdicionarAsync(PlanoTrabalho item)
        {
            var result = await Entity.AddAsync(item);
            return result.Entity;
        }

        public void Atualizar(PlanoTrabalho item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

    }
}
