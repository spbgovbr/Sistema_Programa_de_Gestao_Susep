using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel.PessoaAggregate;

namespace Susep.SISRH.Infrastructure.EntityConfigurations
{
    public class PessoaEntityTypeConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoa");

            builder.HasKey(p => p.PessoaId);

            builder.Ignore(p => p.Id)
                   .Ignore(p => p.RequestedHashCode)
                   .Ignore(p => p.CargaHoraria);

            builder.Property(p => p.PessoaId).HasColumnName("pessoaId");
            builder.Property(p => p.Nome).HasColumnName("pesNome");
            builder.Property(p => p.CargaHorariaDb).HasColumnName("cargaHoraria");
            builder.Property(p => p.TipoFuncaoId).HasColumnName("tipoFuncaoId");
            builder.Property(p => p.Cpf).HasColumnName("pesCPF");
            builder.Property(p => p.Email).HasColumnName("pesEmail");
            builder.Property(p => p.MatriculaSiape).HasColumnName("pesMatriculaSiape");





        }

    }
}
