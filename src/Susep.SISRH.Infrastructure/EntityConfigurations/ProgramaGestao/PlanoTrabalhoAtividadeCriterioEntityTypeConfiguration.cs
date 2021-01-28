using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoAtividadeCriterioEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoAtividadeCriterio>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoAtividadeCriterio> builder)
        {
            builder.ToTable("PlanoTrabalhoAtividadeCriterio", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoAtividadeCriterioId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoAtividadeCriterioId)
                .HasColumnName("planoTrabalhoAtividadeCriterioId")
                .ValueGeneratedOnAdd(); ;
            
            builder.Property(p => p.PlanoTrabalhoAtividadeId).HasColumnName("planoTrabalhoAtividadeId");
            builder.Property(p => p.CriterioId).HasColumnName("criterioId");

            builder.HasOne(p => p.Criterio)
                   .WithMany(p => p.CriteriosAtividadesPlanos)
                   .HasForeignKey(p => p.CriterioId)
                   .HasConstraintName("FK_PlanoTrabalhoCriterioAtividade_Criterio");

            builder.HasOne(p => p.PlanoTrabalhoAtividade)
                   .WithMany(p => p.Criterios)
                   .HasForeignKey(p => p.PlanoTrabalhoAtividadeId)
                   .HasConstraintName("FK_PlanoTrabalhoCriterioAtividade_PlanoTrabalhoAtividade");

        }

    }
}
