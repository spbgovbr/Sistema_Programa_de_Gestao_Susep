using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class CatalogoItemCatalogoEntityTypeConfiguration : IEntityTypeConfiguration<CatalogoItemCatalogo>
    {
        public void Configure(EntityTypeBuilder<CatalogoItemCatalogo> builder)
        {
            builder.ToTable("CatalogoItemCatalogo", "ProgramaGestao");

            builder.HasKey(p => p.CatalogoItemCatalogoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.CatalogoItemCatalogoId)
                   .HasColumnName("catalogoItemCatalogoId")
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.CatalogoId).HasColumnName("catalogoId");

            builder.Property(p => p.ItemCatalogoId).HasColumnName("itemCatalogoId");

            builder.HasOne(p => p.Catalogo)
                   .WithMany(p => p.ItensCatalogo)
                   .HasForeignKey(p => p.CatalogoId)
                   .HasConstraintName("FK_CatalogoItemCatalogo_Catalogo");

            builder.HasOne(p => p.ItemCatalogo)
                   .WithMany(p => p.Catalogos)
                   .HasForeignKey(p => p.ItemCatalogoId)
                   .HasConstraintName("FK_CatalogoItemCatalogo_ItemCatalogo");

        }

    }
}
