using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoAtividadeAssuntoEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoAtividadeAssunto>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoAtividadeAssunto> builder)
        {
            builder.ToTable("PlanoTrabalhoAtividadeAssunto", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoAtividadeAssuntoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoAtividadeAssuntoId)
                   .HasColumnName("planoTrabalhoAtividadeAssuntoId")
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.PlanoTrabalhoAtividadeId).HasColumnName("planoTrabalhoAtividadeId");

            builder.Property(p => p.AssuntoId).HasColumnName("assuntoId");

            builder.HasOne(p => p.Assunto)
                   .WithMany()
                   .HasForeignKey(p => p.AssuntoId)
                   .HasConstraintName("FK_PlanoTrabalhoAtividadeAssunto_Assunto");

        }

    }
}
