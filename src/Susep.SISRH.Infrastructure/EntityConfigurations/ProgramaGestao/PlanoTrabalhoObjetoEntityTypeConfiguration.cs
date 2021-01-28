using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoObjetoEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoObjeto>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoObjeto> builder)
        {
            builder.ToTable("PlanoTrabalhoObjeto", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoObjetoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoObjetoId)
                   .HasColumnName("planoTrabalhoObjetoId")
                   .ValueGeneratedOnAdd(); ;
            
            builder.Property(p => p.PlanoTrabalhoId).HasColumnName("planoTrabalhoId");
            builder.Property(p => p.ObjetoId).HasColumnName("objetoId");

            builder.HasOne(p => p.PlanoTrabalho)
                   .WithMany(p => p.Objetos)
                   .HasForeignKey(p => p.PlanoTrabalhoId)
                   .HasConstraintName("FK_PlanoTrabalhoObjeto_PlanoTrabalho");

            builder.HasOne(p => p.Objeto)
                   .WithMany()
                   .HasForeignKey(p => p.ObjetoId)
                   .HasConstraintName("FK_PlanoTrabalhoObjeto_Objeto");

        }

    }
}
