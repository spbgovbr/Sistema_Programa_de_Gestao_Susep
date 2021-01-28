using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoCustoEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoCusto>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoCusto> builder)
        {
            builder.ToTable("PlanoTrabalhoCusto", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoCustoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoCustoId)
                .HasColumnName("planoTrabalhoCustoId")
                .ValueGeneratedOnAdd();
            
            builder.Property(p => p.PlanoTrabalhoId).HasColumnName("planoTrabalhoId");
            builder.Property(p => p.Valor).HasColumnName("valor");
            builder.Property(p => p.Descricao).HasColumnName("descricao");

            builder.HasOne(p => p.PlanoTrabalho)
                   .WithMany(p => p.Custos)
                   .HasForeignKey(p => p.PlanoTrabalhoId)
                   .HasConstraintName("FK_PlanoTrabalhoCusto_PlanoTrabalho");

            builder.HasOne(p => p.PlanoTrabalhoObjeto)
                   .WithMany(p => p.Custos)
                   .HasForeignKey(p => p.PlanoTrabalhoObjetoId)
                   .HasConstraintName("FK_PlanoTrabalhoCusto_PlanoTrabalhoObjeto");

        }
    }
}
