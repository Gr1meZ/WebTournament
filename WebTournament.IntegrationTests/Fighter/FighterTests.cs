using CustomExceptionsLibrary;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.Belt.CreateBelt;
using WebTournament.Application.Club.CreateClub;
using WebTournament.Application.Fighter;
using WebTournament.Application.Fighter.CreateFighter;
using WebTournament.Application.Fighter.GetFighter;
using WebTournament.Application.Fighter.GetFighterList;
using WebTournament.Application.Fighter.RemoveFighter;
using WebTournament.Application.Fighter.UpdateFighter;
using WebTournament.Application.Tournament.CreateTournament;
using WebTournament.Application.Trainer.CreateTrainer;
using WebTournament.Application.WeightCategorie.CreateWeightCategorie;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Extensions;

namespace WebTournament.IntegrationTests.Fighter;

public class FighterTests : BaseIntegrationTest
{

    public FighterTests(WebApplicationFactory factory) : base(factory)
    {
    }
    
      [Fact]
    public async Task Fighter_Must_BeCreated()
    {
        var createFighterCommand = new CreateFighterCommand()
        {
            TournamentId =  await GetRandomTournamentAsync(),
            BeltId = await CreateRandomBeltAsync(), 
            TrainerId = await CreateRandomTrainerAsync(),
            WeightCategorieId = await CreateRandomWeightCategorieIdAsync(),
            Name = "Иван", 
            Surname = "Иванов", 
            BirthDate = DateTime.UtcNow.AddYears(-10), 
            Country = "Россия",
            City = "Москва", 
            Gender = Gender.Male.MapToString()
        };
        
        await Sender.Send(createFighterCommand);
        
        var fighter = await DbContext.Fighters.FirstOrDefaultAsync(x => x.Name == createFighterCommand.Name);
        
        Assert.NotNull(fighter);
    }
    
    [Fact]
    public async Task Fighter_AlreadyExists_ThrowsException()
    {
        var createFighterCommand = new CreateFighterCommand()
        {
            TournamentId =  await GetRandomTournamentAsync(), 
            BeltId = await CreateRandomBeltAsync(), 
            TrainerId = await CreateRandomTrainerAsync(),
            WeightCategorieId = await CreateRandomWeightCategorieIdAsync(),
            Name = "Денис", 
            Surname = "Артемович", 
            BirthDate = DateTime.UtcNow.AddYears(-20), 
            Country = "Россия",
            City = "Москва", 
            Gender = Gender.Male.MapToString()
        };
        
        await Sender.Send(createFighterCommand);
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(createFighterCommand));
    }
    
    [Fact]
    public async Task GetFighter_IsValid()
    {
        var fighterId = await DbContext.Fighters.Select(x => x.Id).FirstOrDefaultAsync();
        var getFighterQuery = new GetFighterQuery(fighterId);
        
        var fighterResponse = await Sender.Send(getFighterQuery);
        
        Assert.NotNull(fighterResponse);
    }
    
    [Fact]
    public async Task GetFighter_NotFound_ThrowsException()
    {
        var getFighterQuery = new GetFighterQuery(Guid.NewGuid());
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(getFighterQuery));
    }
    
    [Fact]
    public async Task GetFighterList_Must_ReturnValidPagedResponse()
    {
        var getFighterListBySearch = new GetFighterListQuery(await GetRandomTournamentAsync()) { Search = "Москва" };
        var getFighterListByOrder = new GetFighterListQuery(await GetRandomTournamentAsync()) {OrderColumn = "city", OrderDir = "desc", Search = ""};
        
        var searchResponse = await Sender.Send(getFighterListBySearch);
        var orderColumnResponse = await Sender.Send(getFighterListByOrder);
        
        var dataBySearch = await DbContext.Fighters.Where(x => x.Name.Contains(getFighterListBySearch.Search)).ToListAsync();
        var dataByOrder = await DbContext.Fighters.OrderByDescending(x => x.Name).ToListAsync();
        
        var expectedSearch = dataBySearch.Select(fighter => Mapper.Map<FighterResponse>(fighter)).ToArray();
        var expectedOrder = dataByOrder.Select(fighter => Mapper.Map<FighterResponse>(fighter)).ToArray();
        
        Assert.Equal(searchResponse.Metadata.TotalItemCount, expectedSearch.Length);
        Assert.Equal(orderColumnResponse.Metadata.TotalItemCount, expectedOrder.Length);

    }
    
    [Fact]
    public async Task Fighter_Must_BeRemoved()
    {
        var createFighterCommand = new CreateFighterCommand()
        {
            TournamentId =  await GetRandomTournamentAsync(), 
            BeltId = await CreateRandomBeltAsync(), 
            TrainerId = await CreateRandomTrainerAsync(),
            WeightCategorieId = await CreateRandomWeightCategorieIdAsync(),
            Name = "Артем", 
            Surname = "Сергеев", 
            BirthDate = DateTime.UtcNow.AddYears(-10), 
            Country = "Россия",
            City = "Москва", 
            Gender = Gender.Male.MapToString()
        };
      
        
        await Sender.Send(createFighterCommand);

        var fighterId =  await DbContext.Fighters
            .Where(x => x.Name == createFighterCommand.Name && x.Surname == createFighterCommand.Surname && x.TrainerId == createFighterCommand.TrainerId)
            .Select(x => x.Id).FirstOrDefaultAsync();

        var removeCommand = new RemoveFighterCommand(fighterId);

        await Sender.Send(removeCommand);

        var isDeleted = !await DbContext.Fighters.AnyAsync(x => x.Id == fighterId);
        
        Assert.True(isDeleted);
    }
    
    [Fact]
    public async Task Fighter_Must_BeUpdated()
    {
        var fighterId = await DbContext.Fighters.Select(x => x.Id).FirstOrDefaultAsync();
        var updateCommand = new UpdateFighterCommand()
        {
            Id = fighterId,
            TournamentId =  await GetRandomTournamentAsync(), 
            BeltId = await CreateRandomBeltAsync(), 
            TrainerId = await CreateRandomTrainerAsync(),
            WeightCategorieId = await CreateRandomWeightCategorieIdAsync(),
            Name = "Иванов", 
            Surname = "Иванович", 
            BirthDate = DateTime.UtcNow.AddYears(-10), 
            Country = "Россия",
            City = "Москва", 
            Gender = Gender.Male.MapToString()
        };
        
        await Sender.Send(updateCommand);

        var updatedFighter = await DbContext.Fighters.FindAsync(fighterId);
        
        
        Assert.Equal(updateCommand.Id, updatedFighter.Id);
        Assert.Equal(updateCommand.TournamentId, updatedFighter.TournamentId);
        Assert.Equal(updateCommand.BeltId, updatedFighter.BeltId);
        Assert.Equal(updateCommand.TrainerId, updatedFighter.TrainerId);
        Assert.Equal(updateCommand.WeightCategorieId, updatedFighter.WeightCategorieId);
        Assert.Equal(updateCommand.Name, updatedFighter.Name);
        Assert.Equal(updateCommand.Surname, updatedFighter.Surname);
        Assert.Equal(updateCommand.BirthDate, updatedFighter.BirthDate);
        Assert.Equal(updateCommand.Country, updatedFighter.Country);
        Assert.Equal(updateCommand.City, updatedFighter.City);
        Assert.Equal(updateCommand.Gender, updatedFighter.Gender.MapToString());
    }

    private async Task<Guid> GetRandomTournamentAsync()
    {
        var anyTournament = await DbContext.Tournaments.FirstOrDefaultAsync();
        if (anyTournament is not null)
        {
            return anyTournament.Id;
        }
        
        var tournamentCmd = new CreateTournamentCommand()
            { Name = Guid.NewGuid().ToString(), Address = Guid.NewGuid().ToString(), StartDate = DateTime.UtcNow.AddDays(new Random().Next(0, 50)) };
        
        await Sender.Send(tournamentCmd);
        
        return await DbContext.Tournaments
            .Where(x => x.Name == tournamentCmd.Name && x.StartDate == tournamentCmd.StartDate)
            .Select(x => x.Id).FirstOrDefaultAsync();
    }
    
    private async Task<Guid> CreateRandomBeltAsync()
    {
        var beltCmd = new CreateBeltCommand()
            { BeltNumber = new Random().Next(0, 10), ShortName = Guid.NewGuid().ToString(), FullName = Guid.NewGuid().ToString()};
        
        await Sender.Send(beltCmd);
        
        return await DbContext.Belts
            .Where(x => x.BeltNumber == beltCmd.BeltNumber && x.ShortName == beltCmd.ShortName)
            .Select(x => x.Id).FirstOrDefaultAsync();
    }
    
    private async Task<Guid> CreateRandomTrainerAsync()
    {
        await Sender.Send(new CreateClubCommand() { Name = Guid.NewGuid().ToString() });
        var clubId = await DbContext.Clubs.Select(x => x.Id).FirstOrDefaultAsync();
        
        var trainerCmd = new CreateTrainerCommand()
            { 
                Name = Guid.NewGuid().ToString(),
                Surname  = Guid.NewGuid().ToString(),
                Patronymic = Guid.NewGuid().ToString(), 
                Phone  = Guid.NewGuid().ToString(),
                ClubId = clubId
            };
        
        await Sender.Send(trainerCmd);
        
        return await DbContext.Trainers
            .Where(x => x.Name == trainerCmd.Name && x.Surname == trainerCmd.Surname
                                                  && x.ClubId == clubId && x.Phone == trainerCmd.Phone)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }
    
    private async Task<Guid> CreateRandomWeightCategorieIdAsync()
    {
        var ageGroupId = await DbContext.AgeGroups.Select(x => x.Id).FirstOrDefaultAsync();
        if (ageGroupId == Guid.Empty)
        {
            var ageGroupCmd = new CreateAgeGroupCommand(){Name = "Test", MaxAge = 10, MinAge = 3};
            await Sender.Send(ageGroupCmd);
            ageGroupId = await DbContext.AgeGroups.Select(x => x.Id).FirstOrDefaultAsync();
        }
        
        var weightCategorieCommand = new CreateWeightCategorieCommand()
        { 
            Gender = Gender.Male.MapToString(),
            MaxWeight = new Random().Next(0, 500),
            WeightName = Guid.NewGuid().ToString(),
            AgeGroupId = ageGroupId
        };
        
        await Sender.Send(weightCategorieCommand);
        
        return await DbContext.WeightCategories
            .Where(x => x.Gender == GenderExtension.ParseEnum(weightCategorieCommand.Gender) && x.MaxWeight == weightCategorieCommand.MaxWeight
                                                  && x.AgeGroupId == ageGroupId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }
}