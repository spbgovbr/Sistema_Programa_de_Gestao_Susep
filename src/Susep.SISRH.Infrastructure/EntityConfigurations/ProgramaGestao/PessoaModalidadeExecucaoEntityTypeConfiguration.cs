using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class PessoaModalidadeExecucaoEntityTypeConfiguration : IEntityTypeConfiguration<PessoaModalidadeExecucao>
    {
        public void Configure(EntityTypeBuilder<PessoaModalidadeExecucao> builder)
        {
            builder.ToTable("PessoaModalidadeExecucao", "ProgramaGestao");

            builder.HasKey(p => p.PessoaModalidadeExecucaoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.PessoaId).HasColumnName("pessoaId");
            builder.Property(p => p.ModalidadeExecucaoId).HasColumnName("modalidadeExecucaoId");


            builder.HasOne(p => p.Pessoa)
                   .WithMany(p => p.ModalidadesExecucao)
                   .HasForeignKey(p => p.PessoaId)
                   .HasConstraintName("FK_PessoaModalidadeExecucao_Pessoa");

            builder.HasOne(p => p.ModalidadeExecucao)
                   .WithMany(p => p.PessoasModalidadesExecucao)
                   .HasForeignKey(p => p.ModalidadeExecucaoId)
                   .HasConstraintName("FK_PessoaModalidadeExecucao_ModalidadeExecucao");

        }

    }
}
