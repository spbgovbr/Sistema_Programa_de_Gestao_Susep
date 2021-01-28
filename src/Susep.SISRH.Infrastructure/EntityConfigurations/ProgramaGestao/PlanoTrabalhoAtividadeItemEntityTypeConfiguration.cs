using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoAtividadeItemEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoAtividadeItem>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoAtividadeItem> builder)
        {
            builder.ToTable("PlanoTrabalhoAtividadeItem", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoAtividadeItemId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoAtividadeItemId)
                .HasColumnName("planoTrabalhoAtividadeItemId")
                .ValueGeneratedOnAdd(); ;
            
            builder.Property(p => p.PlanoTrabalhoAtividadeId).HasColumnName("planoTrabalhoAtividadeId");
            builder.Property(p => p.ItemCatalogoId).HasColumnName("itemCatalogoId");

            builder.HasOne(p => p.ItemCatalogo)
                   .WithMany(p => p.PlanosTrabalhoAtividadesItens)
                   .HasForeignKey(p => p.ItemCatalogoId)
                   .HasConstraintName("FK_PlanoTrabalhoItemAtividade_ItemCatalogo");

            builder.HasOne(p => p.PlanoTrabalhoAtividade)
                   .WithMany(p => p.ItensCatalogo)
                   .HasForeignKey(p => p.PlanoTrabalhoAtividadeId)
                   .HasConstraintName("FK_PlanoTrabalhoItemAtividade_PlanoTrabalhoAtividade");

        }

    }
}
