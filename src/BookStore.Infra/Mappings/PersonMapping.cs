using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace BookStore.Infra.Mappings
{
    public class PersonMapping : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Code).IsRequired().HasColumnType("int");

            builder.Property(b => b.Name).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");

            builder.Property(b => b.Email).IsRequired().HasMaxLength(150).HasColumnType("varchar(150)");

            builder.Property(b => b.Type).IsRequired().HasColumnType("smallint");

            builder.Property(b => b.IsActive).IsRequired().HasColumnType("bit");

            builder.ToTable("Persons");
        }
    }
}
