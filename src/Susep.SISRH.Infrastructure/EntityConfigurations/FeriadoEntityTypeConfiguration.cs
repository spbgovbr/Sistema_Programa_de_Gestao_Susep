using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.FeriadoAggregate;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations
{
    public class FeriadoEntityTypeConfiguration : IEntityTypeConfiguration<Feriado>
    {
        public void Configure(EntityTypeBuilder<Feriado> builder)
        {
            builder.ToTable("Feriado");

            builder.HasKey(p => p.FeriadoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.FeriadoId).HasColumnName("feriadoId");
            builder.Property(p => p.Data).HasColumnName("ferData");
            builder.Property(p => p.Fixo).HasColumnName("ferFixo");
            builder.Property(p => p.Descricao).HasColumnName("ferDescricao");
            builder.Property(p => p.UfId).HasColumnName("ufId");





        }

    }
}
