using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tuudio.Infrastructure.Services.Interfaces;
using Tuudio.Infrastructure.Services.Repositories;

namespace Tuudio.Infrastructure.Data;
public static class DbContextExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
    {
        var serverVersion = new MariaDbServerVersion(new Version(10, 11, 8));

        services.AddDbContext<TuudioDbContext>(options => options
            .UseMySql(connectionString, serverVersion)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IClientRepository, ClientRepository>();

    }
}
