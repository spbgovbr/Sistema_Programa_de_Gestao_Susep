using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations
{
    public class UnidadeEntityTypeConfiguration : IEntityTypeConfiguration<Unidade>
    {
        public void Configure(EntityTypeBuilder<Unidade> builder)
        {
            builder.ToTable("VW_UnidadeSiglaCompleta");

            builder.HasKey(p => p.UnidadeId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.UnidadeId).HasColumnName("unidadeId");
            builder.Property(p => p.Sigla).HasColumnName("undSigla");
            builder.Property(p => p.SiglaCompleta).HasColumnName("undSiglaCompleta");            
            builder.Property(p => p.Nome).HasColumnName("undDescricao");
            builder.Property(p => p.UfId).HasColumnName("ufId");

        }

    }
}
