using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Susep.SISRH.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Infrastructure.Factories
{
    public class SISRHDbContextFactory : IDesignTimeDbContextFactory<SISRHDbContext>
    {
        public SISRHDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<SISRHDbContext> builder = new DbContextOptionsBuilder<SISRHDbContext>();
            builder.UseSqlServer("data source=(localdb)\\MSSQLLocalDB;initial catalog=Corretores;Trusted_Connection=True;", x => x.MigrationsHistoryTable("__MigrationsHistory", "dbo"));

            return new SISRHDbContext(builder.Options);
        }
    }
}
