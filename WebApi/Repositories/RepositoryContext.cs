using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Repostories.Config;

namespace WebApi.Repostories
{
    public class RepostoryContext : DbContext
    {
        public RepostoryContext(DbContextOptions options) :
            base(options)
        {
            
        }
        public DbSet<Book> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookConfig());
        }
    }
}