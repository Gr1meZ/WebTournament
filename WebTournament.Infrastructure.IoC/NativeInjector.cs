using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Infrastructure.Data;
using WebTournament.Infrastructure.Identity.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.SeedWork;
using WebTournament.Infrastructure.Data.Repository;
using WebTournament.Infrastructure.Data.UoW;

namespace WebTournament.Infrastructure.IoC
{
    public static class NativeInjector
    {
        public static  IServiceCollection AddCustomServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAgeGroupRepository, AgeGroupRepository>();

            return services;
        }
    }
}