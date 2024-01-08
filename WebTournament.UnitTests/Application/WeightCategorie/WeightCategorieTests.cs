using AutoMapper;
using Moq;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Application.Trainer;
using WebTournament.Application.Trainer.GetTrainer;
using WebTournament.Application.WeightCategorie;
using WebTournament.Application.WeightCategorie.GetWeightCategorie;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Application.WeightCategorie;

public class WeightCategorieTests
{
    private readonly Mock<IWeightCategorieRepository> _weightCategorieRepository = new();

    [Fact]
    public async Task GetTournament_Must_BeSuccessful()
    {
        var getWeightCategorieQuery = new GetWeightCategorieQuery(Guid.NewGuid());
        
        var weightCategorie = await WeightCategorieBuilder.BuildAsync(getWeightCategorieQuery.Id);

        _weightCategorieRepository.Setup(method => method.GetByIdAsync(getWeightCategorieQuery.Id))
            .ReturnsAsync(weightCategorie);
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
        var mapper = config.CreateMapper();
        var weightCategorieResponse = mapper.Map<WeightCategorieResponse>(weightCategorie);
        
        Assert.Equal(weightCategorie.Id, weightCategorieResponse.Id);
        Assert.Equal(weightCategorie.WeightName, weightCategorieResponse.WeightName);
        Assert.Equal(weightCategorie.MaxWeight, weightCategorieResponse.MaxWeight);
        Assert.Equal(weightCategorie.AgeGroupId, weightCategorieResponse.AgeGroupId);
        Assert.Equal(weightCategorie.Gender.MapToString(), weightCategorieResponse.Gender);
        
    }
}