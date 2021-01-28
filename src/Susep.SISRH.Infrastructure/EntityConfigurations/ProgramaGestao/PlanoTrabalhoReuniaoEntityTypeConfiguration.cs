using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoReuniaoEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoReuniao>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoReuniao> builder)
        {
            builder.ToTable("PlanoTrabalhoReuniao", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoReuniaoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoReuniaoId)
                .HasColumnName("planoTrabalhoReuniaoId")
                .ValueGeneratedOnAdd();
            
            builder.Property(p => p.PlanoTrabalhoId).HasColumnName("planoTrabalhoId");
            builder.Property(p => p.Data).HasColumnName("data");
            builder.Property(p => p.Titulo).HasColumnName("titulo");
            builder.Property(p => p.Descricao).HasColumnName("descricao");


            builder.HasOne(p => p.PlanoTrabalho)
                   .WithMany(p => p.Reunioes)
                   .HasForeignKey(p => p.PlanoTrabalhoId)
                   .HasConstraintName("FK_PlanoTrabalhoReuniao_PlanoTrabalho");

            builder.HasOne(p => p.PlanoTrabalhoObjeto)
                   .WithMany(p => p.Reunioes)
                   .HasForeignKey(p => p.PlanoTrabalhoObjetoId)
                   .HasConstraintName("FK_PlanoTrabalhoReuniao_PlanoTrabalhoObjeto");

        }

    }
}
