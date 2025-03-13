using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.Efcore;
using Services;
using Services.Contracts;

public static class ServicesExtentions
{
    public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();
    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerService , LoggerManager>();
}
