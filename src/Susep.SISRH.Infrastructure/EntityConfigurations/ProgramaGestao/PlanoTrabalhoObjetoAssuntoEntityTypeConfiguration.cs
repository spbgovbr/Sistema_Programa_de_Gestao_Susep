using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoObjetoAssuntoEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoObjetoAssunto>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoObjetoAssunto> builder)
        {
            builder.ToTable("PlanoTrabalhoObjetoAssunto", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoObjetoAssuntoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoObjetoAssuntoId)
                   .HasColumnName("planoTrabalhoObjetoAssuntoId")
                   .ValueGeneratedOnAdd(); ;
            
            builder.Property(p => p.PlanoTrabalhoObjetoId).HasColumnName("planoTrabalhoObjetoId");
            builder.Property(p => p.AssuntoId).HasColumnName("assuntoId");

            builder.HasOne(p => p.PlanoTrabalhoObjeto)
                   .WithMany(p => p.Assuntos)
                   .HasForeignKey(p => p.PlanoTrabalhoObjetoId)
                   .HasConstraintName("FK_PlanoTrabalhoObjetoAssunto_PlanoTrabalhoObjeto");

            builder.HasOne(p => p.Assunto)
                   .WithMany()
                   .HasForeignKey(p => p.AssuntoId)
                   .HasConstraintName("FK_PlanoTrabalhoObjetoAssunto_Assunto");

        }

    }
}
