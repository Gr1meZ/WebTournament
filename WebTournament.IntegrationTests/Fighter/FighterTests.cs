using System.IO.Compression;
using CustomExceptionsLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.Belt.CreateBelt;
using WebTournament.Application.Club.CreateClub;
using WebTournament.Application.Fighter;
using WebTournament.Application.Fighter.CreateFighter;
using WebTournament.Application.Fighter.CreateFightersFromExcel;
using WebTournament.Application.Fighter.GetFighter;
using WebTournament.Application.Fighter.GetFighterList;
using WebTournament.Application.Fighter.RemoveAllFighters;
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
    private readonly IWebHostEnvironment? _environment;
    public FighterTests(WebApplicationFactory factory) : base(factory)
    {
        _environment = factory.Services.GetService<IWebHostEnvironment>();
    }
    
      [Fact]
    public async Task Fighter_Must_BeCreated()
    {
        var createFighterCommand = await CreateFighterCommandAsync();
        
        await Sender.Send(createFighterCommand);
        
        var fighter = await DbContext.Fighters
            .FirstOrDefaultAsync(x => x.Name == createFighterCommand.Name && x.Surname == createFighterCommand.Surname);
        
        Assert.NotNull(fighter);
    }
    
    [Fact]
    public async Task Fighter_AlreadyExists_ThrowsException()
    {
        var createFighterCommand = await CreateFighterCommandAsync();
        
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
        var tournamentId = await GetRandomTournamentIdAsync();
        
        var getFighterListByOrder = new GetFighterListQuery(tournamentId) {OrderColumn = "city", OrderDir = "desc", Search = ""};
        
        var orderColumnResponse = await Sender.Send(getFighterListByOrder);
        
        var dataByOrder = await DbContext.Fighters.OrderByDescending(x => x.Name).ToListAsync();
        
        var expectedOrder = dataByOrder.Select(fighter => Mapper.Map<FighterResponse>(fighter)).ToArray();
        
        Assert.Equal(orderColumnResponse.Metadata.TotalItemCount, expectedOrder.Length);

    }
    
    [Fact]
    public async Task Fighter_Must_BeRemoved()
    {
        var createFighterCommand = new CreateFighterCommand()
        {
            TournamentId =  await GetRandomTournamentIdAsync(), 
            BeltId = await CreateRandomBeltIdAsync(), 
            TrainerId = await CreateRandomTrainerIdAsync(),
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
    public async Task AllFighters_Must_BeRemoved()
    {
        var createFighterCommand = await CreateFighterCommandAsync();
        
        await Sender.Send(createFighterCommand);
        
        var removeCommand = new RemoveAllFightersCommand(createFighterCommand.TournamentId.Value);

        await Sender.Send(removeCommand);

        var isDeleted = !await DbContext.Fighters.AnyAsync(x => x.TournamentId == createFighterCommand.TournamentId.Value);
        
        Assert.True(isDeleted);
    }
    
    [Fact]
    public async Task Fighter_Must_BeUpdated()
    {
        var fighterId = await DbContext.Fighters.Select(x => x.Id).FirstOrDefaultAsync();
        var updateCommand = new UpdateFighterCommand()
        {
            Id = fighterId,
            TournamentId =  await GetRandomTournamentIdAsync(), 
            BeltId = await CreateRandomBeltIdAsync(), 
            TrainerId = await CreateRandomTrainerIdAsync(),
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

    [Fact]
    public async Task ExcelFile_Must_BeEmpty()
    {
        var tournamentId = await GetRandomTournamentIdAsync();
        using var stream = new MemoryStream();

        var excelFile = new FormFile(stream,0, 0, "excel", "test.xlsx" );
        var excelFileCmd = new CreateFightersFromExcelCommand(tournamentId, excelFile);

        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(excelFileCmd));
    }
    
    [Fact]
    public async Task ExcelFile_Must_HaveNotValidExtension()
    {
        var tournamentId = await GetRandomTournamentIdAsync();
        using var stream = new MemoryStream();

        var excelFile = new FormFile(stream,0, 10, "excel", "test.doc" );
        var excelFileCmd = new CreateFightersFromExcelCommand(tournamentId, excelFile);

        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(excelFileCmd));
    }
    
    [Fact]
    public async Task ExcelFile_Must_AddFighter()
    {
        await Sender.Send(new CreateAgeGroupCommand() { Name = "Test", MinAge = 5, MaxAge = 7 });
        var ageGroup = await DbContext.AgeGroups.FirstOrDefaultAsync(x => x.MinAge == 5 && x.MaxAge == 7);
        
        await Sender.Send(new CreateBeltCommand() { BeltNumber = 10, ShortName = "гып", FullName = "Test"});
        var belt = await DbContext.Belts.FirstOrDefaultAsync(x => x.BeltNumber == 10);
        
        await Sender.Send(new CreateClubCommand() { Name = "Юнош" });
        var club = await DbContext.Clubs.FirstOrDefaultAsync(x => x.Name == "Юнош");
        
        await Sender.Send(new CreateWeightCategorieCommand() {Gender = Gender.Female.MapToString(), MaxWeight = 25, WeightName = "Test", AgeGroupId = ageGroup.Id});
        var weightCategorie = await DbContext.WeightCategories.FirstOrDefaultAsync(x => x.MaxWeight == 25);
        
        await Sender.Send(new CreateTrainerCommand()
            { Name = "Константин", Surname = "Кочмаров", Patronymic = "Юрьевич", Phone = "123", ClubId = club.Id });
        var trainer = await DbContext.Trainers.FirstOrDefaultAsync(x => x.ClubId == club.Id);
        
        var tournamentId = await GetRandomTournamentIdAsync();
        
        var filePath = Path.Combine(_environment.WebRootPath, "requests", "validRequest.xlsx");
        await using var stream = File.OpenRead(filePath);
        var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
        
        var excelFileCmd = new CreateFightersFromExcelCommand(tournamentId, file);
        await Sender.Send(excelFileCmd);
        
        var isFighterCreated = await DbContext.Fighters.AnyAsync(x =>
            x.TournamentId == tournamentId && x.TrainerId == trainer.Id &&
            x.BeltId == belt.Id && x.WeightCategorieId == weightCategorie.Id);
        
        Assert.True(isFighterCreated);
    }


    [Fact]
    public async Task ExcelFile_Have_NotValidWorksheet()
    {
        var tournamentId = await GetRandomTournamentIdAsync();

        var filePath = Path.Combine(_environment.WebRootPath, "requests", "falseWorksheet.xlsx");
        await using var stream = File.OpenRead(filePath);
        var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
        
        var excelFileCmd = new CreateFightersFromExcelCommand(tournamentId, file);

        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(excelFileCmd));
    }
}