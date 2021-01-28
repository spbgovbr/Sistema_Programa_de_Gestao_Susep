using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PactoTrabalhoAtividadeEntityTypeConfiguration : IEntityTypeConfiguration<PactoTrabalhoAtividade>
    {
        public void Configure(EntityTypeBuilder<PactoTrabalhoAtividade> builder)
        {
            builder.ToTable("PactoTrabalhoAtividade", "ProgramaGestao");

            builder.HasKey(p => p.PactoTrabalhoAtividadeId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PactoTrabalhoAtividadeId)
                .HasColumnName("pactoTrabalhoAtividadeId")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PactoTrabalhoId).HasColumnName("pactoTrabalhoId");
            builder.Property(p => p.ItemCatalogoId).HasColumnName("itemCatalogoId");
            builder.Property(p => p.Quantidade).HasColumnName("quantidade");
            builder.Property(p => p.TempoPrevistoPorItem).HasColumnName("tempoPrevistoPorItem");
            builder.Property(p => p.TempoPrevistoTotal).HasColumnName("tempoPrevistoTotal");
            builder.Property(p => p.DataInicio).HasColumnName("dataInicio");
            builder.Property(p => p.DataFim).HasColumnName("dataFim");
            builder.Property(p => p.TempoRealizado).HasColumnName("tempoRealizado");
            builder.Property(p => p.TempoHomologado).HasColumnName("tempoHomologado");            
            builder.Property(p => p.SituacaoId).HasColumnName("situacaoId");
            builder.Property(p => p.Descricao).HasColumnName("descricao");
            builder.Property(p => p.ConsideracoesConclusao).HasColumnName("consideracoesConclusao");
            builder.Property(p => p.Nota).HasColumnName("nota");
            builder.Property(p => p.Justificativa).HasColumnName("justificativa");

            builder.HasOne(p => p.PactoTrabalho)
                   .WithMany(p => p.Atividades)
                   .HasForeignKey(p => p.PactoTrabalhoId)
                   .HasConstraintName("FK_PactoTrabalhoAtividade_PactoTrabalho");

            builder.HasOne(p => p.ItemCatalogo)
                   .WithMany(p => p.PactosTrabalhoAtividades)
                   .HasForeignKey(p => p.ItemCatalogoId)
                   .HasConstraintName("FK_PactoTrabalhoAtividade_ItemCatalogo");

            builder.HasOne(p => p.Situacao)
                   .WithMany(p => p.PactosTrabalhoAtividades)
                   .HasForeignKey(p => p.SituacaoId)
                   .HasConstraintName("FK_PactoTrabalhoAtividade_Situacao");

            builder.HasMany(p => p.Assuntos)
                   .WithOne()
                   .HasForeignKey(p => p.PactoTrabalhoAtividadeId)
                   .HasConstraintName("FK_PactoTrabalhoAtividadeAssunto_PactoTrabalhoAtividade");

        }

    }
}
