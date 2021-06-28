using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Infra.Mappings
{
    public class BookMapping : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Code).IsRequired().HasColumnType("int");

            builder.Property(b => b.Name).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");

            builder.Property(b => b.ReleaseYear).IsRequired().HasColumnType("smallint");

            builder.Property(b => b.Isbn).IsRequired().HasMaxLength(13).HasColumnType("varchar(13)");
            
            builder.HasOne(b => b.Author);
            builder.ToTable("Books");
        }
    }
}
