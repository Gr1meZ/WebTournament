using CustomExceptionsLibrary;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.WeightCategorie;
using WebTournament.Application.WeightCategorie.CreateWeightCategorie;
using WebTournament.Application.WeightCategorie.GetWeightCategorie;
using WebTournament.Application.WeightCategorie.GetWeightCategorieList;
using WebTournament.Application.WeightCategorie.RemoveWeightCategorie;
using WebTournament.Application.WeightCategorie.UpdateWeightCategorie;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Extensions;

namespace WebTournament.IntegrationTests.WeightCategorie;

public class WeightCategorieTests : BaseIntegrationTest
{
    public WeightCategorieTests(WebApplicationFactory factory) : base(factory)
    {
    }
    
     [Fact]
    public async Task WeightCategorie_Must_BeCreated()
    {
        var createWeightCategorieCommand = await CreateWeightCategorieCommandAsync();

        await Sender.Send(createWeightCategorieCommand);

        var weightCategorie = await DbContext.WeightCategories
            .FirstOrDefaultAsync(x => x.WeightName == createWeightCategorieCommand.WeightName);

        Assert.NotNull(weightCategorie);
    }

    [Fact]
    public async Task WeightCategorie_AlreadyExists_ThrowsException()
    {
        var createWeightCategorieCommand = await CreateWeightCategorieCommandAsync();

        await Sender.Send(createWeightCategorieCommand);

        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(createWeightCategorieCommand));
    }

    [Fact]
    public async Task GetWeightCategorie_IsValid()
    {
        var weightCategorieId = await DbContext.WeightCategories.Select(x => x.Id).FirstOrDefaultAsync();
        var getWeightCategorieQuery = new GetWeightCategorieQuery(weightCategorieId);

        var weightCategorieResponse = await Sender.Send(getWeightCategorieQuery);

        Assert.NotNull(weightCategorieResponse);
    }

    [Fact]
    public async Task GetWeightCategorie_NotFound_ThrowsException()
    {
        var getWeightCategorieQuery = new GetWeightCategorieQuery(Guid.NewGuid());

        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(getWeightCategorieQuery));
    }

    [Fact]
    public async Task GetWeightCategorieList_Must_ReturnValidPagedResponse()
    {
        var ageGroupId = await GetRandomAgeGroupIdAsync();
        await Sender.Send(new CreateWeightCategorieCommand() { WeightName = "10kg", MaxWeight = 10, Gender = Gender.Male.ToString(), AgeGroupId = ageGroupId});
        await Sender.Send(new CreateWeightCategorieCommand() { WeightName = "11kg", MaxWeight = 11, Gender = Gender.Male.ToString(), AgeGroupId = ageGroupId});
        await Sender.Send(new CreateWeightCategorieCommand() { WeightName = "12kg", MaxWeight = 12, Gender = Gender.Male.ToString(), AgeGroupId = ageGroupId});

        var getWeightCategorieListBySearch = new GetWeightCategorieListQuery() { Search = "kg" };
        var getWeightCategorieListByOrder = new GetWeightCategorieListQuery() { OrderColumn = "name", OrderDir = "desc", Search = "" };

        var searchResponse = await Sender.Send(getWeightCategorieListBySearch);
        var orderColumnResponse = await Sender.Send(getWeightCategorieListByOrder);

        var dataBySearch = await DbContext.WeightCategories.Where(x => x.WeightName
            .Contains(getWeightCategorieListBySearch.Search)).ToListAsync();
        var dataByOrder = await DbContext.WeightCategories.OrderByDescending(x => x.MaxWeight).ToListAsync();

        var expectedSearch = dataBySearch.Select(weightCategorie => Mapper.Map<WeightCategorieResponse>(weightCategorie)).ToArray();
        var expectedOrder = dataByOrder.Select(weightCategorie => Mapper.Map<WeightCategorieResponse>(weightCategorie)).ToArray();

        Assert.Equal(searchResponse.Metadata.TotalItemCount, expectedSearch.Length);
        Assert.Equal(orderColumnResponse.Metadata.TotalItemCount, expectedOrder.Length);
    }

    [Fact]
    public async Task WeightCategorie_Must_BeRemoved()
    {
        var createWeightCategorieCommand = await CreateWeightCategorieCommandAsync();


        await Sender.Send(createWeightCategorieCommand);

        var weightCategorieId = await DbContext.WeightCategories
            .Where(x => x.WeightName == createWeightCategorieCommand.WeightName)
            .Select(x => x.Id).FirstOrDefaultAsync();

        var removeCommand = new RemoveWeightCategorieCommand(weightCategorieId);

        await Sender.Send(removeCommand);

        var isDeleted = !await DbContext.WeightCategories.AnyAsync(x => x.Id == weightCategorieId);

        Assert.True(isDeleted);
    }

    [Fact]
    public async Task WeightCategorie_Must_BeUpdated()
    {
        var ageGroupId = await GetRandomAgeGroupIdAsync();
        var weightCategorieId = await DbContext.WeightCategories
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
        
        var updateCommand = new UpdateWeightCategorieCommand() 
            { 
                Id = weightCategorieId, 
                WeightName = "Test", 
                MaxWeight = 50, 
                Gender = Gender.Female.MapToString(), 
                AgeGroupId = ageGroupId
            };

        await Sender.Send(updateCommand);

        var updatedWeightCategorie = await DbContext.WeightCategories.FindAsync(weightCategorieId);


        Assert.Equal(updateCommand.Id, updatedWeightCategorie.Id);
        Assert.Equal(updateCommand.WeightName, updatedWeightCategorie.WeightName);
        Assert.Equal(updateCommand.MaxWeight, updatedWeightCategorie.MaxWeight);
        Assert.Equal(updateCommand.Gender, updatedWeightCategorie.Gender.MapToString());
        Assert.Equal(updateCommand.AgeGroupId, updatedWeightCategorie.AgeGroupId);

    }
}