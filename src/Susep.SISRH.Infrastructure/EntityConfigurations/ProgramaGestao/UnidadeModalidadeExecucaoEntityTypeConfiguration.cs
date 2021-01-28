using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.UnidadeAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class UnidadeModalidadeExecucaoEntityTypeConfiguration : IEntityTypeConfiguration<UnidadeModalidadeExecucao>
    {
        public void Configure(EntityTypeBuilder<UnidadeModalidadeExecucao> builder)
        {
            builder.ToTable("UnidadeModalidadeExecucao", "ProgramaGestao");

            builder.HasKey(p => p.UnidadeModalidadeExecucaoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.UnidadeId).HasColumnName("unidadeId");
            builder.Property(p => p.ModalidadeExecucaoId).HasColumnName("modalidadeExecucaoId");


            builder.HasOne(p => p.Unidade)
                   .WithMany(p => p.ModalidadesExecucao)
                   .HasForeignKey(p => p.UnidadeId)
                   .HasConstraintName("FK_UnidadeModalidadeExecucao_Unidade");

            builder.HasOne(p => p.ModalidadeExecucao)
                   .WithMany(p => p.UnidadesModalidadesExecucao)
                   .HasForeignKey(p => p.ModalidadeExecucaoId)
                   .HasConstraintName("FK_UnidadeModalidadeExecucao_ModalidadeExecucao");

        }

    }
}
