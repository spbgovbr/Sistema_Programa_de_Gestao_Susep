using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PlanoTrabalhoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PlanoTrabalhoAtividadeCandidatoEntityTypeConfiguration : IEntityTypeConfiguration<PlanoTrabalhoAtividadeCandidato>
    {
        public void Configure(EntityTypeBuilder<PlanoTrabalhoAtividadeCandidato> builder)
        {
            builder.ToTable("PlanoTrabalhoAtividadeCandidato", "ProgramaGestao");

            builder.HasKey(p => p.PlanoTrabalhoAtividadeCandidatoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PlanoTrabalhoAtividadeCandidatoId)
                .HasColumnName("planoTrabalhoAtividadeCandidatoId")
                .ValueGeneratedOnAdd();

            builder.Property(p => p.PlanoTrabalhoAtividadeId).HasColumnName("planoTrabalhoAtividadeId");
            builder.Property(p => p.PessoaId).HasColumnName("pessoaId");
            builder.Property(p => p.SituacaoId).HasColumnName("situacaoId");
            builder.Property(p => p.TermoAceite).HasColumnName("termoAceite");

            builder.HasOne(p => p.PlanoTrabalhoAtividade)
                   .WithMany(p => p.Candidatos)
                   .HasForeignKey(p => p.PlanoTrabalhoAtividadeId)
                   .HasConstraintName("FK_PlanoTrabalhoAtividadeCandidato_PlanoTrabalhoAtividade");

            builder.HasOne(p => p.Pessoa)
                   .WithMany(p => p.Candidaturas)
                   .HasForeignKey(p => p.PessoaId)
                   .HasConstraintName("FK_PlanoTrabalhoAtividadeCandidato_Pessoa");

            builder.HasOne(p => p.Situacao)
                   .WithMany(p => p.PlanoTrabalhoAtividadeCandidatos)
                   .HasForeignKey(p => p.SituacaoId)
                   .HasConstraintName("FK_PlanoTrabalhoAtividadeCandidato_Situacao");
        }

    }
}
