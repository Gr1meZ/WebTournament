using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Infrastructure.Data;
using WebTournament.Infrastructure.Identity.Models;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebTournament.Infrastructure.IoC
{
    public static class NativeInjector
    {
        public static  IServiceCollection AddCustomServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }
    }
}