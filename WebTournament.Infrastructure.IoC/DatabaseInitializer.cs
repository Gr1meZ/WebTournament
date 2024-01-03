using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Infrastructure.Data.Context;
using WebTournament.Infrastructure.Identity.Models;
namespace WebTournament.Infrastructure.IoC;

public static class DatabaseInitializer
{
    public static  IServiceCollection AddDatabase(this IServiceCollection services,
        IConfiguration configuration, string environment)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            if (environment == "Production")
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });
           


        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        return services;
    }
}