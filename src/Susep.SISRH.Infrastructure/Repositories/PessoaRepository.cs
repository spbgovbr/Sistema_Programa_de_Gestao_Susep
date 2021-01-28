using Microsoft.EntityFrameworkCore;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;
using Susep.SISRH.Infrastructure.Contexts;
using SUSEP.Framework.Data.Concrete.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Susep.SISRH.Infrastructure.Repositories
{
    public class PessoaRepository : SqlServerRepository<Pessoa>, IPessoaRepository
    {
        private readonly SISRHDbContext _context;

        public PessoaRepository(SISRHDbContext context) : base(context) {
            _context = context;
        }

        public async Task<Pessoa> ObterAsync(long pessoaId)
        {
            return await Entity
                .Include(p => p.Unidade)
                .Include(p => p.PactosTrabalho)
                .FirstOrDefaultAsync(p => p.PessoaId == pessoaId);
        }

        public async Task<Pessoa> ObterPorCriteriosAsync(string email, string cpf)
        {
            List<Pessoa> retorno = null;
            IQueryable<Pessoa> pessoa = Entity
                .Include(p => p.Unidade);

            //Filtra inicialmente somente pelo CPF
            if (!string.IsNullOrEmpty(cpf))
            {
                pessoa = pessoa.Where(p => p.Cpf == cpf);
                retorno = await pessoa.ToListAsync();
            }

            //Na sequência, filtra pelo email
            if (!string.IsNullOrEmpty(email))
            {
                //Se tiver retornado mais de um pelo CPF, filtra também pelo email
                if (retorno.Count() > 1)
                    retorno = retorno.Where(p => p.Email == email).ToList();
                //Se o filtro não tiver retornado pelo CPF, busca apenas pelo email para ver se acha
                else if(!retorno.Any())
                    retorno = await pessoa.Where(p => p.Email == email).ToListAsync();
            }

            return retorno.FirstOrDefault();
        }
    }
}
