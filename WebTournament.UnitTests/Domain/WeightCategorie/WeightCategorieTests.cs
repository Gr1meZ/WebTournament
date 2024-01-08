using CustomExceptionsLibrary;
using Moq;
using WebTournament.Application.WeightCategorie.CreateWeightCategorie;
using WebTournament.Application.WeightCategorie.UpdateWeightCategorie;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Domain.WeightCategorie;

public class WeightCategorieTests
{
     private readonly Mock<IWeightCategorieRepository> _weightCategorieRepository = new();
     
    [Fact]
    public async Task WeightCategorie_Must_BeCreated()
    {
        var createWeightCategorieCommand = new CreateWeightCategorieCommand()
        {
            Gender = Gender.Male.MapToString(),
            MaxWeight = 10,
            AgeGroupId = Guid.NewGuid(),
            WeightName = "10kg"
        };
        
        _weightCategorieRepository.Setup(method =>
                method.IsExistsAsync(10, Gender.Male.MapToString(), Guid.NewGuid()))
            .ReturnsAsync(false);

        var weightCategorie = await WeightCategorieBuilder.BuildAsync(Guid.NewGuid(), createWeightCategorieCommand.AgeGroupId.Value);
        
        Assert.Equal(weightCategorie.WeightName, createWeightCategorieCommand.WeightName);
        Assert.Equal(weightCategorie.MaxWeight, createWeightCategorieCommand.MaxWeight);
        Assert.Equal(weightCategorie.AgeGroupId, createWeightCategorieCommand.AgeGroupId);
        Assert.Equal(weightCategorie.Gender.MapToString(), createWeightCategorieCommand.Gender);
    }
    
    [Fact]
    public async Task WeightCategorie_AlreadyExists()
    {
        var createWeightCategorieCommand = new CreateWeightCategorieCommand()
        {
            Gender = Gender.Male.MapToString(),
            MaxWeight = 10,
            AgeGroupId = Guid.NewGuid(),
            WeightName = "10kg"
        };
        
        _weightCategorieRepository.Setup(method =>
                method.IsExistsAsync(10, Gender.Male.MapToString(), createWeightCategorieCommand.AgeGroupId.Value))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ValidationException>(() => WebTournament.Domain.Objects.WeightCategorie.WeightCategorie.CreateAsync(
            Guid.NewGuid(), createWeightCategorieCommand.AgeGroupId.Value, createWeightCategorieCommand.MaxWeight.Value, 
            createWeightCategorieCommand.WeightName, GenderExtension.ParseEnum(createWeightCategorieCommand.Gender), _weightCategorieRepository.Object));
    }
    
    [Fact]
    public async Task WeightCategorie_Must_BeChanged()
    {
        var id = Guid.NewGuid();
        
        var updateWeightCategorieCommand = new UpdateWeightCategorieCommand()
        {
            Id = id,
            Gender = Gender.Female.MapToString(),
            MaxWeight = 12,
            AgeGroupId = Guid.NewGuid(),
            WeightName = "12kg"
        };
        
        var weightCategorie = await WeightCategorieBuilder.BuildAsync(id);
        
        _weightCategorieRepository.Setup(method => method.GetByIdAsync(updateWeightCategorieCommand.Id))
            .ReturnsAsync(weightCategorie);

        weightCategorie.Change(updateWeightCategorieCommand.AgeGroupId.Value, updateWeightCategorieCommand.MaxWeight.Value,
            updateWeightCategorieCommand.WeightName, GenderExtension.ParseEnum(updateWeightCategorieCommand.Gender));
        
        Assert.Equal(weightCategorie.Id, updateWeightCategorieCommand.Id);
        Assert.Equal(weightCategorie.WeightName, updateWeightCategorieCommand.WeightName);
        Assert.Equal(weightCategorie.MaxWeight, updateWeightCategorieCommand.MaxWeight);
        Assert.Equal(weightCategorie.AgeGroupId, updateWeightCategorieCommand.AgeGroupId);
        Assert.Equal(weightCategorie.Gender.MapToString(), updateWeightCategorieCommand.Gender);
    }
}