using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoAtividadeEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoAtividade>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoAtividade> builder)
        {
            builder.ToTable("PlanoTrabalhoAtividade", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoAtividadeId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoAtividadeId)
                .HasColumnName("planoTrabalhoAtividadeId")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PlanoTrabalhoId).HasColumnName("planoTrabalhoId");
            builder.Property(p => p.ModalidadeExecucaoId).HasColumnName("modalidadeExecucaoId");
            builder.Property(p => p.QuantidadeColaboradores).HasColumnName("quantidadeColaboradores");
            builder.Property(p => p.Descricao).HasColumnName("descricao");

            builder.HasOne(p => p.PlanoTrabalho)
                   .WithMany(p => p.Atividades)
                   .HasForeignKey(p => p.PlanoTrabalhoId)
                   .HasConstraintName("FK_PlanoTrabalhoAtividade_PlanoTrabalho");

            builder.HasOne(p => p.ModalidadeExecucao)
                   .WithMany(p => p.PlanosTrabalhoAtividades)
                   .HasForeignKey(p => p.ModalidadeExecucaoId)
                   .HasConstraintName("FK_PlanoTrabalhoAtividade_ModalidadeExecucao");

            builder.HasMany(p => p.Assuntos)
                   .WithOne()
                   .HasForeignKey(p => p.PlanoTrabalhoAtividadeId)
                   .HasConstraintName("FK_PlanoTrabalhoAtividadeAssunto_PlanoTrabalhoAtividade");

        }

    }
}
