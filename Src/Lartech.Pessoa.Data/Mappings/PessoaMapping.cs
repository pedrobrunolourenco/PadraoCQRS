using Lartech.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lartech.Data.Mappings
{
    public class PessoaMapping : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.HasKey(p => new { p.Id });
            builder.Ignore(p => p.ListaErros);
            builder.Ignore(p => p.ListaTelefones);
            builder.Ignore(p => p.ValidationResult);
            builder.Property(p => p.Nome).IsRequired().HasColumnType("varchar").HasMaxLength(100);
            builder.Property(p => p.CPF).IsRequired().HasColumnType("varchar").HasMaxLength(11);
            builder.Property(p => p.DataNascimento).IsRequired().HasColumnType("datetime").HasColumnName("DataNascimento");
            builder.Property(p => p.Ativo).HasColumnType("bit").HasColumnName("Ativo");

            builder.HasMany(p => p.Telefones)
                  .WithOne(p => p.Pessoa)
                  .HasForeignKey(t => t.PessoaId);


            builder.ToTable("Pessoas");
        }
    }
}
