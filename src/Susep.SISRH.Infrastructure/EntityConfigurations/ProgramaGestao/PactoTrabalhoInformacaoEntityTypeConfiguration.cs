using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PactoTrabalhoInformacaoEntityTypeConfiguration : IEntityTypeConfiguration<PactoTrabalhoInformacao>
    {
        public void Configure(EntityTypeBuilder<PactoTrabalhoInformacao> builder)
        {
            builder.ToTable("PactoTrabalhoInformacao", "ProgramaGestao");

            builder.HasKey(p => p.PactoTrabalhoInformacaoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PactoTrabalhoInformacaoId)
                .HasColumnName("pactoTrabalhoInformacaoId")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PactoTrabalhoId).HasColumnName("pactoTrabalhoId");
            builder.Property(p => p.Informacao).HasColumnName("informacao");
            builder.Property(p => p.DataRegistro).HasColumnName("dataRegistro");
            builder.Property(p => p.ResponsavelRegistro).HasColumnName("responsavelRegistro");

            builder.HasOne(p => p.PactoTrabalho)
                   .WithMany(p => p.Informacoes)
                   .HasForeignKey(p => p.PactoTrabalhoId);

        }

    }
}
