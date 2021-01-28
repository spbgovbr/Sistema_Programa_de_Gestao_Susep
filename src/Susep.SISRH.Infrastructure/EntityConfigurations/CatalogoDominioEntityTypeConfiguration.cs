using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Susep.SISRH.Domain.AggregatesModel;
using Susep.SISRH.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Susep.SISRH.Infrastructure.EntityConfigurations.ProgramaGestao
{
    public class CatalogoDominioEntityTypeConfiguration : IEntityTypeConfiguration<CatalogoDominio>
    {
        public void Configure(EntityTypeBuilder<CatalogoDominio> builder)
        {
            builder.ToTable("dbo", "ProgramaGestao");

            builder.HasKey(p => p.CatalogoDominioId);

            builder.Property(p => p.CatalogoDominioId)
                   .HasColumnName("catalogoDominioId");

            builder.Property(p => p.Classificacao)
                   .HasColumnName("classificacao")
                   .HasMaxLength(50);

            builder.Property(p => p.Descricao)
                   .HasColumnName("descricao")
                   .HasMaxLength(250);

            builder.Property(p => p.Ativo)
                   .HasColumnName("ativo");
            
        }

    }
}
