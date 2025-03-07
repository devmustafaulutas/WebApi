using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Models;

namespace WebApi.Repostories.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book {Id=1, Title="Karagöz ve hacivat",Price=75},
                new Book {Id=2, Title="Mesnevi",Price=200},
                new Book {Id=3, Title="Devlet",Price=375}
            );
        }
    }
}