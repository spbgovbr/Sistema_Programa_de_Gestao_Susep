using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class CatalogoEntityTypeConfiguration : IEntityTypeConfiguration<Catalogo>
    {
        public void Configure(EntityTypeBuilder<Catalogo> builder)
        {
            builder.ToTable("Catalogo", "ProgramaGestao");

            builder.HasKey(p => p.CatalogoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.CatalogoId)
                   .HasColumnName("catalogoId");


            builder.HasOne(p => p.Unidade)
                   .WithMany(p => p.Catalogos)
                   .HasForeignKey(p => p.UnidadeId)
                   .HasConstraintName("FK_Catalogo_Unidade");

        }

    }
}
