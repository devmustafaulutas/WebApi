using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repostories
{
    public class RepostoryContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;
    }
}