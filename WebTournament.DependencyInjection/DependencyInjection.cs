using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataAccess.MSSQL;
using DataAccess.IdentityModels;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static  IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString,
               b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
               .EnableSensitiveDataLogging(),ServiceLifetime.Transient);


            services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());


            return services;
        }
    }
}