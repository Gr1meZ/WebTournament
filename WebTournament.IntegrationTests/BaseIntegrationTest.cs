using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.IntegrationTests;

public class BaseIntegrationTest : IClassFixture<WebApplicationFactory>
{
    protected readonly ISender Sender;
    protected readonly ApplicationDbContext DbContext;
    protected readonly IMapper Mapper;
    
    public BaseIntegrationTest(WebApplicationFactory factory)
    {
        var scope = factory.Services.CreateScope();
        Sender = scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        Mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
    }
}