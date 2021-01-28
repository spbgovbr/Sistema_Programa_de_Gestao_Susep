using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoAtividadeCandidatoHistoricoEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoAtividadeCandidatoHistorico>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoAtividadeCandidatoHistorico> builder)
        {
            builder.ToTable("PlanoTrabalhoAtividadeCandidatoHistorico", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoAtividadeCandidatoHistoricoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoAtividadeCandidatoHistoricoId)
                .HasColumnName("planoTrabalhoAtividadeCandidatoHistoricoId")
                .ValueGeneratedOnAdd();
            
            builder.Property(p => p.PlanoTrabalhoAtividadeCandidatoId).HasColumnName("planoTrabalhoAtividadeCandidatoId");
            builder.Property(p => p.SituacaoId).HasColumnName("situacaoId");
            builder.Property(p => p.Data).HasColumnName("data");
            builder.Property(p => p.Descricao).HasColumnName("descricao");
            builder.Property(p => p.ResponsavelOperacao).HasColumnName("responsavelOperacao");            

            builder.HasOne(p => p.PlanoTrabalhoAtividadeCandidato)
                   .WithMany(p => p.Historico)
                   .HasForeignKey(p => p.PlanoTrabalhoAtividadeCandidatoId)
                   .HasConstraintName("FK_PlanoTrabalhoAtividadeCandidatoHistorico_PlanoTrabalhoAtividadeCandidato");

            builder.HasOne(p => p.Situacao)
                   .WithMany(p => p.PlanoTrabalhoAtividadeCandidatoHistoricos)
                   .HasForeignKey(p => p.SituacaoId)
                   .HasConstraintName("FK_PlanoTrabalhoAtividadeCandidatoHistorico_Situacao");

        }

    }
}
