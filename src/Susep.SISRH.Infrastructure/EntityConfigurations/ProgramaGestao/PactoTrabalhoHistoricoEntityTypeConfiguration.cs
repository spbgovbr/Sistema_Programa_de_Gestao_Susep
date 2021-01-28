using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PactoTrabalhoHistoricoEntityTypeConfiguration : IEntityTypeConfiguration<PactoTrabalhoHistorico>
    {
        public void Configure(EntityTypeBuilder<PactoTrabalhoHistorico> builder)
        {
            builder.ToTable("PactoTrabalhoHistorico", "ProgramaGestao");

            builder.HasKey(p => p.PactoTrabalhoHistoricoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PactoTrabalhoHistoricoId)
                   .HasColumnName("pactoTrabalhoHistoricoId")
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.PactoTrabalhoId).HasColumnName("pactoTrabalhoId");
            builder.Property(p => p.SituacaoId).HasColumnName("situacaoId");
            builder.Property(p => p.Observacoes).HasColumnName("observacoes");
            builder.Property(p => p.ResponsavelOperacao).HasColumnName("responsavelOperacao");



            builder.HasOne(p => p.PactoTrabalho)
                   .WithMany(p => p.Historico)
                   .HasForeignKey(p => p.PactoTrabalhoId)
                   .HasConstraintName("FK_PactoTrabalhoHistorico_PactoTrabalho");

            builder.HasOne(p => p.Situacao)
                   .WithMany(p => p.HistoricoPactosTrabalho)
                   .HasForeignKey(p => p.SituacaoId)
                   .HasConstraintName("FK_PactoTrabalhoHistorico_Situacao");


        }

    }
}
