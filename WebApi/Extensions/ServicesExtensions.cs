using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.Efcore;
using Services;
using Services.Contracts;
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

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager , RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager , ServiceManager>();
    }    
}