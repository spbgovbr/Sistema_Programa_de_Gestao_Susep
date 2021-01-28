using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class AssuntoEntityTypeConfiguration : IEntityTypeConfiguration<Assunto>
    {
        public void Configure(EntityTypeBuilder<Assunto> builder)
        {
            builder.ToTable("Assunto", "ProgramaGestao");

            builder.HasKey(p => p.AssuntoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.Valor)
                   .HasColumnName("valor")
                   .HasMaxLength(100);

            builder.Property(p => p.Ativo)
                   .HasColumnName("ativo");

            builder.HasOne(p => p.AssuntoPai)
                   .WithMany()
                   .HasForeignKey(p => p.AssuntoPaiId)
                   .HasConstraintName("FK_Assunto_AssuntoPai");

        }

    }
}
