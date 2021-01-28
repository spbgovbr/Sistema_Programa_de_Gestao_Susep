using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalho>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalho> builder)
        {
            builder.ToTable("PlanoTrabalho", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoId).HasColumnName("planoTrabalhoId");
            builder.Property(p => p.UnidadeId).HasColumnName("unidadeId");
            builder.Property(p => p.DataInicio).HasColumnName("dataInicio");
            builder.Property(p => p.DataFim).HasColumnName("dataFim");
            builder.Property(p => p.SituacaoId).HasColumnName("situacaoId");
            builder.Property(p => p.PrazoComparecimento).HasColumnName("tempoComparecimento");
            builder.Property(p => p.TotalServidoresSetor).HasColumnName("totalServidoresSetor");
            builder.Property(p => p.TempoFaseHabilitacao).HasColumnName("tempoFaseHabilitacao");
            builder.Property(p => p.TermoAceite).HasColumnName("termoAceite");


            builder.HasOne(p => p.Unidade)
                   .WithMany(p => p.PlanosTrabalho)
                   .HasForeignKey(p => p.UnidadeId)
                   .HasConstraintName("FK_PlanoTrabalho_Unidade");

            builder.HasOne(p => p.Situacao)
                   .WithMany(p => p.PlanosTrabalho)
                   .HasForeignKey(p => p.SituacaoId)
                   .HasConstraintName("FK_PlatoTrabalho_Situacao");

        }

    }
}
