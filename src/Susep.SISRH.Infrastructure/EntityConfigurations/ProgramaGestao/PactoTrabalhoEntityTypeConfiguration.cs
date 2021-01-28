using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PactoTrabalhoEntityTypeConfiguration : IEntityTypeConfiguration<PactoTrabalho>
    {
        public void Configure(EntityTypeBuilder<PactoTrabalho> builder)
        {
            builder.ToTable("PactoTrabalho", "ProgramaGestao");

            builder.HasKey(p => p.PactoTrabalhoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Ignore(p => p.DiasNaoUteis);

            builder.Property(p => p.PactoTrabalhoId).HasColumnName("pactoTrabalhoId");
            builder.Property(p => p.PlanoTrabalhoId).HasColumnName("planoTrabalhoId");
            builder.Property(p => p.UnidadeId).HasColumnName("unidadeId");
            builder.Property(p => p.PessoaId).HasColumnName("pessoaId");
            builder.Property(p => p.DataInicio).HasColumnName("dataInicio");
            builder.Property(p => p.DataFim).HasColumnName("dataFim");
            builder.Property(p => p.ModalidadeExecucaoId).HasColumnName("formaExecucaoId");
            builder.Property(p => p.SituacaoId).HasColumnName("situacaoId");
            builder.Property(p => p.TermoAceite).HasColumnName("termoAceite");
            builder.Property(p => p.CargaHorariaDiaria).HasColumnName("cargaHorariaDiaria");
            builder.Property(p => p.PercentualExecucao).HasColumnName("percentualExecucao");
            builder.Property(p => p.RelacaoPrevistoRealizado).HasColumnName("relacaoPrevistoRealizado");
            builder.Property(p => p.TempoTotalDisponivel).HasColumnName("tempoTotalDisponivel");            

            builder.HasOne(p => p.Unidade)
                   .WithMany(p => p.PactosTrabalho)
                   .HasForeignKey(p => p.UnidadeId)
                   .HasConstraintName("FK_PactoTrabalho_Unidade");

            builder.HasOne(p => p.Pessoa)
                   .WithMany(p => p.PactosTrabalho)
                   .HasForeignKey(p => p.PessoaId)
                   .HasConstraintName("FK_PactoTrabalho_Pessoa");

        }

    }
}
