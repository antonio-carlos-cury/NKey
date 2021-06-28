using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infra.Mappings
{
    public class AuthorMapping : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Code).IsRequired().HasColumnType("int");

            builder.Property(a => a.Name).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");

            builder.Property(a => a.Email).IsRequired().HasMaxLength(150).HasColumnType("varchar(150)");

            builder.Property(a => a.IsActive).IsRequired().HasColumnType("bit");

            builder.HasMany(a => a.Books).WithOne(b => b.Author);

            builder.ToTable("Authors");
        }
    }
}
