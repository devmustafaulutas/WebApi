using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repostories
{
    public class RepostoryContext : DbContext
    {
        public RepostoryContext(DbContextOptions options) :
            base(options)
        {
            
        }
        public DbSet<Book> Books { get; set; } = null!;
    }
}