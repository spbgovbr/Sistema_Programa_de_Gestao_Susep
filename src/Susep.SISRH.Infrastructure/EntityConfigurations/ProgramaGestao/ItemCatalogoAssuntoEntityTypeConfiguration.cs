using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class ItemCatalogoAssuntoEntityTypeConfiguration : IEntityTypeConfiguration<ItemCatalogoAssunto>
    {
        public void Configure(EntityTypeBuilder<ItemCatalogoAssunto> builder)
        {
            builder.ToTable("ItemCatalogoAssunto", "ProgramaGestao");

            builder.HasKey(p => p.ItemCatalogoAssuntoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.ItemCatalogoAssuntoId)
                   .HasColumnName("itemCatalogoAssuntoId")
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.ItemCatalogoId).HasColumnName("itemCatalogoId");

            builder.Property(p => p.AssuntoId).HasColumnName("assuntoId");

            builder.HasOne(p => p.Assunto)
                   .WithMany()
                   .HasForeignKey(p => p.AssuntoId)
                   .HasConstraintName("FK_ItemCatalogoAssunto_Assunto");

        }

    }
}
