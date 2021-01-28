using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PactoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PactoAtividadePlanoObjetoEntityTypeConfiguration : IEntityTypeConfiguration<PactoAtividadePlanoObjeto>
    {
        public void Configure(EntityTypeBuilder<PactoAtividadePlanoObjeto> builder)
        {
            builder.ToTable("PactoAtividadePlanoObjeto", "ProgramaGestao");

            builder.HasKey(p => p.PactoAtividadePlanoObjetoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PactoAtividadePlanoObjetoId)
                .HasColumnName("pactoAtividadePlanoObjetoId")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PlanoTrabalhoObjetoId).HasColumnName("planoTrabalhoObjetoId");
            builder.Property(p => p.PactoTrabalhoAtividadeId).HasColumnName("pactoTrabalhoAtividadeId");

            builder.HasOne(p => p.PactoTrabalhoAtividade)
                   .WithMany(p => p.Objetos)
                   .HasForeignKey(p => p.PactoTrabalhoAtividadeId)
                   .HasConstraintName("FK_PactoAtividadePlanoObjeto_PactoTrabalhoAtividade");

            builder.HasOne(p => p.PlanoTrabalhoObjeto)
                   .WithMany()
                   .HasForeignKey(p => p.PlanoTrabalhoObjetoId)
                   .HasConstraintName("FK_PactoAtividadePlanoObjeto_PlanoTrabalhoObjeto");

        }

    }
}
