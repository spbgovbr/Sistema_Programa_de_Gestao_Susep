using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoHistoricoEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoHistorico>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoHistorico> builder)
        {
            builder.ToTable("PlanoTrabalhoHistorico", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoHistoricoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoHistoricoId)
                   .HasColumnName("planoTrabalhoHistoricoId")
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.PlanoTrabalhoId).HasColumnName("planoTrabalhoId");
            builder.Property(p => p.SituacaoId).HasColumnName("situacaoId");
            builder.Property(p => p.Observacoes).HasColumnName("observacoes");
            builder.Property(p => p.ResponsavelOperacao).HasColumnName("responsavelOperacao");



            builder.HasOne(p => p.PlanoTrabalho)
                   .WithMany(p => p.Historico)
                   .HasForeignKey(p => p.PlanoTrabalhoId)
                   .HasConstraintName("FK_PlanoTrabalhoHistorico_PlanoTrabalho");

            builder.HasOne(p => p.Situacao)
                   .WithMany(p => p.HistoricoPlanosTrabalho)
                   .HasForeignKey(p => p.SituacaoId)
                   .HasConstraintName("FK_PlanoTrabalhoHistorico_Situacao");


        }

    }
}
