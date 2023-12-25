using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace WebTournament.Presentation.MVC.ProgramExtensions;

public static class HealthCheckExtension
{
    public static IServiceCollection AddCustomizedHealthCheck(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("DefaultConnection")!);

        services.AddHealthChecksUI(opt =>
        {
            opt.SetEvaluationTimeInSeconds(15); 
        }).AddPostgreSqlStorage(configuration.GetConnectionString("DefaultConnection")!);

        return services;
    }

    public static IEndpointRouteBuilder UseCustomizedHealthCheck(this IEndpointRouteBuilder endpoints, IWebHostEnvironment env)
    {
        endpoints.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        endpoints.MapHealthChecksUI(setup => { setup.UIPath = "/health-ui"; });
        return endpoints;
    }
}