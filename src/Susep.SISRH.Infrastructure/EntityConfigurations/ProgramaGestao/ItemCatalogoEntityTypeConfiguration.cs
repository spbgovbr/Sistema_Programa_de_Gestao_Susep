using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.CatalogoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class ItemCatalogoEntityTypeConfiguration : IEntityTypeConfiguration<ItemCatalogo>
    {
        public void Configure(EntityTypeBuilder<ItemCatalogo> builder)
        {
            builder.ToTable("ItemCatalogo", "ProgramaGestao");

            builder.HasKey(p => p.ItemCatalogoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode)
                   .Ignore(p => p.TempoExecucaoPreviamenteDefinido);

            builder.Property(p => p.ItemCatalogoId).HasColumnName("itemCatalogoId");
            builder.Property(p => p.Titulo).HasColumnName("titulo");
            builder.Property(p => p.FormaCalculoTempoItemCatalogoId).HasColumnName("calculoTempoId");
            builder.Property(p => p.PermiteTrabalhoRemoto).HasColumnName("permiteRemoto");
            builder.Property(p => p.TempoExecucaoPresencial).HasColumnName("tempoPresencial");
            builder.Property(p => p.TempoExecucaoRemoto).HasColumnName("tempoRemoto");
            builder.Property(p => p.Descricao).HasColumnName("descricao");
            builder.Property(p => p.Complexidade).HasColumnName("complexidade");
            builder.Property(p => p.DefinicaoComplexidade).HasColumnName("definicaoComplexidade");
            builder.Property(p => p.EntregasEsperadas).HasColumnName("entregasEsperadas");

            builder.HasOne(p => p.FormaCalculoTempoItemCatalogo)
                   .WithMany(p => p.ItensCatalogo)
                   .HasForeignKey(p => p.FormaCalculoTempoItemCatalogoId)
                   .HasConstraintName("FK_ItemCatalogo_CalculoTempo");

            builder.HasMany(p => p.Assuntos)
                   .WithOne()
                   .HasForeignKey(p => p.ItemCatalogoId)
                   .HasConstraintName("FK_ItemCatalogoAssunto_ItemCatalogo");
        }

    }
}
