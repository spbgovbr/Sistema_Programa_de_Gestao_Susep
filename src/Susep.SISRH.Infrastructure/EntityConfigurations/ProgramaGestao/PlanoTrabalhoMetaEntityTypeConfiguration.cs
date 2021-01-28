using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoMetaEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoMeta>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoMeta> builder)
        {
            builder.ToTable("PlanoTrabalhoMeta", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoMetaId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoMetaId)
                .HasColumnName("planoTrabalhoMetaId")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PlanoTrabalhoId).HasColumnName("planoTrabalhoId");
            builder.Property(p => p.Meta).HasColumnName("meta");
            builder.Property(p => p.Indicador).HasColumnName("indicador");
            builder.Property(p => p.Descricao).HasColumnName("descricao");

            builder.HasOne(p => p.PlanoTrabalho)
                   .WithMany(p => p.Metas)
                   .HasForeignKey(p => p.PlanoTrabalhoId)
                   .HasConstraintName("FK_PlanoTrabalhoMeta_PlanoTrabalho");

        }

    }
}
