using Lartech.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lartech.Data.Mappings
{
    public class TelefoneMapping : IEntityTypeConfiguration<Telefone>
    {
        public void Configure(EntityTypeBuilder<Telefone> builder)
        {
            builder.HasKey(t => new { t.Id });
            builder.Ignore(t => t.ListaErros);
            builder.Ignore(p => p.ValidationResult);
            builder.Property(t => t.Tipo).HasColumnType("int").IsRequired().HasColumnName("Tipo");
            builder.Property(t => t.Numero).HasColumnType("varchar").IsRequired().HasMaxLength(11);

            builder.HasOne(t => t.Pessoa)
                   .WithMany(t => t.Telefones);

            builder.ToTable("Telefones");

        }
    }
}
