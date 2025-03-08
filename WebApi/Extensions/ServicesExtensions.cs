using Microsoft.EntityFrameworkCore;
using Repositories.Efcore;
namespace WebApi.Extentions
{
    public static class ServicesExtentions
    {
        // Hangi sınıfı genişletiyor isek this anahtar kelimesiyle o sınıfı veriyoruz
        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("sqlConnection")));

    }
    
}