using DataAccess.Abstract;
using DataAccess.Common.IdentityModels;
using DataAccess.MSSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Business.Abstract;
using WebTournament.Business.Services;

namespace DependencyInjection
{
    public static class DependencyInjection
    {
        public static  IServiceCollection AddServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString,
               b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
               .EnableSensitiveDataLogging(),ServiceLifetime.Transient);


            services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IAgeGroupService, AgeGroupService>();
            services.AddScoped<IBeltService, BeltService>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IFighterService, FighterService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<ITrainerService, TrainerService>();
            services.AddScoped<IWeightCategorieService, WeightCategorieService>();
            services.AddScoped<IExcelParserService, ExcelParserService>();


            return services;
        }
    }
}