using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.AgendamentoAggregate;
using Susep.SISRH.Domain.AggregatesModel.AssuntoAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class AgendamentoEntityTypeConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.ToTable("AgendamentoPresencial", "ProgramaGestao");

            builder.HasKey(p => p.AgendamentoId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode);

            builder.Property(p => p.AgendamentoId)
                   .HasColumnName("agendamentoPresencialId").ValueGeneratedOnAdd();

            builder.Property(p => p.PessoaId)
                   .HasColumnName("pessoaId");

            builder.Property(p => p.DataAgendada)
                   .HasColumnName("dataAgendada");

            builder.HasOne(p => p.Pessoa)
                   .WithMany(it => it.Agendamentos)
                   .HasForeignKey(p => p.PessoaId);

        }

    }
}
