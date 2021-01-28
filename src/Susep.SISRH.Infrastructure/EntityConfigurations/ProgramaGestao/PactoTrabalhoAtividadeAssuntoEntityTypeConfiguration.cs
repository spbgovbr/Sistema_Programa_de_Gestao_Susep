using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PactoTrabalhoAtividadeAssuntoEntityTypeConfiguration : IEntityTypeConfiguration<PactoTrabalhoAtividadeAssunto>
    {
        public void Configure(EntityTypeBuilder<PactoTrabalhoAtividadeAssunto> builder)
        {
            builder.ToTable("PactoTrabalhoAtividadeAssunto", "ProgramaGestao");

            builder.HasKey(p => p.PactoTrabalhoAtividadeAssuntoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PactoTrabalhoAtividadeAssuntoId)
                   .HasColumnName("pactoTrabalhoAtividadeAssuntoId")
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.PactoTrabalhoAtividadeId).HasColumnName("pactoTrabalhoAtividadeId");

            builder.Property(p => p.AssuntoId).HasColumnName("assuntoId");

            builder.HasOne(p => p.Assunto)
                   .WithMany()
                   .HasForeignKey(p => p.AssuntoId)
                   .HasConstraintName("FK_PactoTrabalhoAtividadeAssunto_Assunto");

        }

    }
}
