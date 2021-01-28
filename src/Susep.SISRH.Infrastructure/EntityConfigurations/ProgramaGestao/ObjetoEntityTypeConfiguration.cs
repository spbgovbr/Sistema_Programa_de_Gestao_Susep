using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.ObjetoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class ObjetoEntityTypeConfiguration : IEntityTypeConfiguration<Objeto>
    {
        public void Configure(EntityTypeBuilder<Objeto> builder)
        {
            builder.ToTable("Objeto", "ProgramaGestao");

            builder.HasKey(p => p.ObjetoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.Chave)
                   .HasColumnName("chave")
                   .HasMaxLength(20);

            builder.Property(p => p.Descricao)
                   .HasColumnName("descricao")
                   .HasMaxLength(100);

            builder.Property(p => p.Tipo)
                   .HasColumnName("tipo");

            builder.Property(p => p.Ativo)
                    .HasColumnName("ativo");

        }

    }
}
