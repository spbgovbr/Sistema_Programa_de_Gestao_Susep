using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PactoTrabalhoDeclaracaoEntityTypeConfiguration : IEntityTypeConfiguration<PactoTrabalhoDeclaracao>
    {
        public void Configure(EntityTypeBuilder<PactoTrabalhoDeclaracao> builder)
        {
            builder.ToTable("PactoTrabalhoDeclaracao", "ProgramaGestao");

            builder.HasKey(p => p.PactoTrabalhoDeclaracaoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PactoTrabalhoDeclaracaoId)
                .HasColumnName("pactoTrabalhoDeclaracaoId")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PactoTrabalhoId).HasColumnName("pactoTrabalhoId");
            builder.Property(p => p.DeclaracaoId).HasColumnName("declaracaoId");
            builder.Property(p => p.DataExibicao).HasColumnName("dataExibicao");
            builder.Property(p => p.Aceita).HasColumnName("aceita");
            builder.Property(p => p.ResponsavelRegistro).HasColumnName("responsavelRegistro");
            builder.Property(p => p.DataRegistro).HasColumnName("dataRegistro");

            builder.HasOne(p => p.PactoTrabalho)
                   .WithMany(p => p.Declaracoes)
                   .HasForeignKey(p => p.PactoTrabalhoId)
                   .HasConstraintName("FK_PactoTrabalhoDeclaracao_PactoTrabalho");

            builder.HasOne(p => p.Declaracao)
                   .WithMany(p => p.Declaracoes)
                   .HasForeignKey(p => p.DeclaracaoId)
                   .HasConstraintName("FK_PactoTrabalhoDeclaracao_Declaracao");

        }

    }
}
