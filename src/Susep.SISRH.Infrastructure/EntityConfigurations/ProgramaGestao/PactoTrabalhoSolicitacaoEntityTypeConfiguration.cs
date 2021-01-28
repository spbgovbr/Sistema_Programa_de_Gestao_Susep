using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PactoTrabalhoSolicitacaoEntityTypeConfiguration : IEntityTypeConfiguration<PactoTrabalhoSolicitacao>
    {
        public void Configure(EntityTypeBuilder<PactoTrabalhoSolicitacao> builder)
        {
            builder.ToTable("PactoTrabalhoSolicitacao", "ProgramaGestao");

            builder.HasKey(p => p.PactoTrabalhoSolicitacaoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PactoTrabalhoSolicitacaoId)
                .HasColumnName("pactoTrabalhoSolicitacaoId")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PactoTrabalhoId).HasColumnName("pactoTrabalhoId");
            builder.Property(p => p.TipoSolicitacaoId).HasColumnName("tipoSolicitacaoId");
            builder.Property(p => p.DataSolicitacao).HasColumnName("dataSolicitacao");
            builder.Property(p => p.Solicitante).HasColumnName("solicitante");
            builder.Property(p => p.DadosSolicitacao).HasColumnName("dadosSolicitacao");
            builder.Property(p => p.ObservacoesSolicitante).HasColumnName("observacoesSolicitante");
            builder.Property(p => p.Analisado).HasColumnName("analisado");
            builder.Property(p => p.DataAnalise).HasColumnName("dataAnalise");
            builder.Property(p => p.Analista).HasColumnName("analista");
            builder.Property(p => p.Aprovado).HasColumnName("aprovado");
            builder.Property(p => p.ObservacoesAnalista).HasColumnName("observacoesAnalista");

            builder.HasOne(p => p.PactoTrabalho)
                   .WithMany(p => p.Solicitacoes)
                   .HasForeignKey(p => p.PactoTrabalhoId)
                   .HasConstraintName("FK_PactoTrabalhoSolicitacao_PactoTrabalho");

            builder.HasOne(p => p.TipoSolicitacao)
                   .WithMany(p => p.PactosTrabalhoSolicitacoes)
                   .HasForeignKey(p => p.TipoSolicitacaoId)
                   .HasConstraintName("FK_PactoTrabalhoSolicitacao_ItemCatalogo");

        }

    }
}
