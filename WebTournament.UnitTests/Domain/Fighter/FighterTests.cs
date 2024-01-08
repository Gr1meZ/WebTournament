using CustomExceptionsLibrary;
using Moq;
using WebTournament.Application.Fighter.CreateFighter;
using WebTournament.Application.Fighter.UpdateFighter;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Domain.Fighter;

public class FighterTests
{
    private readonly Mock<IFighterRepository> _fighterRepository = new();
     
    [Fact]
    public async Task Fighter_Must_BeCreated()
    {
        var createFighterCommand = new CreateFighterCommand()
        {
            TournamentId = Guid.NewGuid(), 
            BeltId = Guid.NewGuid(), 
            TrainerId = Guid.NewGuid(),
            WeightCategorieId = Guid.NewGuid(),
            Name = "Иван", 
            Surname = "Иванов", 
            BirthDate = DateTime.UtcNow.AddYears(-10), 
            Country = "Россия",
            City = "Москва", 
            Gender = Gender.Male.MapToString()
        };
        
        _fighterRepository.Setup(method => method.IsExistsAsync(createFighterCommand.Surname, createFighterCommand.Name, 
                createFighterCommand.City, createFighterCommand.TournamentId.Value))
            .ReturnsAsync(false);

        var fighter = await FighterBuilder.BuildAsync(Guid.NewGuid(), createFighterCommand.WeightCategorieId.Value, 
            createFighterCommand.BeltId.Value, createFighterCommand.TrainerId.Value, createFighterCommand.BirthDate, createFighterCommand.TournamentId.Value);

        Assert.Equal(fighter.TournamentId, createFighterCommand.TournamentId);
        Assert.Equal(fighter.BeltId, createFighterCommand.BeltId);
        Assert.Equal(fighter.TrainerId, createFighterCommand.TrainerId);
        Assert.Equal(fighter.WeightCategorieId, createFighterCommand.WeightCategorieId);
        Assert.Equal(fighter.Name, createFighterCommand.Name);
        Assert.Equal(fighter.Surname, createFighterCommand.Surname);
        Assert.Equal(fighter.BirthDate, createFighterCommand.BirthDate);
        Assert.Equal(fighter.Country, createFighterCommand.Country);
        Assert.Equal(fighter.City, createFighterCommand.City);
        Assert.Equal(fighter.Gender.MapToString(), createFighterCommand.Gender);

    }
    
    [Fact]
    public async Task Fighter_AlreadyExists()
    {
        var tournamentId = Guid.NewGuid();
        var createFighterCommand = new CreateFighterCommand()
        {
            TournamentId = tournamentId, 
            BeltId = Guid.NewGuid(), 
            TrainerId = Guid.NewGuid(),
            WeightCategorieId = Guid.NewGuid(),
            Name = "Иван", 
            Surname = "Иванов", 
            BirthDate = DateTime.UtcNow.AddYears(-10), 
            Country = "Россия",
            City = "Москва", 
            Gender = Gender.Male.MapToString()
        };
        
        _fighterRepository.Setup(method => method.IsExistsAsync(createFighterCommand.Surname, createFighterCommand.Name, 
                createFighterCommand.City, createFighterCommand.TournamentId.Value))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<ValidationException>(() => WebTournament.Domain.Objects.Fighter.Fighter.CreateAsync(Guid.NewGuid(),
            tournamentId, createFighterCommand.WeightCategorieId.Value, createFighterCommand.BeltId.Value,createFighterCommand.TrainerId.Value,
            null, "Иван", "Иванов", createFighterCommand.BirthDate, "Россия", "Москва", Gender.Male.MapToString(), 
            _fighterRepository.Object));
    }
    
    [Fact]
    public async Task Fighter_Must_BeChanged()
    {
        var id = Guid.NewGuid();
        
        var updateFighterCommand = new UpdateFighterCommand()
        {
            Id = id,
            TournamentId = Guid.NewGuid(), 
            BeltId = Guid.NewGuid(), 
            TrainerId = Guid.NewGuid(),
            WeightCategorieId = Guid.NewGuid(),
            Name = "Артём", 
            Surname = "Морозов", 
            BirthDate = DateTime.UtcNow.AddYears(-12), 
            Country = "Беларусь",
            City = "Минск", 
            Gender = Gender.Male.MapToString()
        };
        
        var fighter = await FighterBuilder.BuildAsync(id);

        _fighterRepository.Setup(method => method.GetByIdAsync(updateFighterCommand.Id))
            .ReturnsAsync(fighter);

        fighter.Change(updateFighterCommand.Name,
            updateFighterCommand.BirthDate, updateFighterCommand.BeltId.Value, updateFighterCommand.City, updateFighterCommand.Country, 
            updateFighterCommand.Gender, updateFighterCommand.Surname, updateFighterCommand.TournamentId.Value, 
            updateFighterCommand.TrainerId.Value, updateFighterCommand.WeightCategorieId.Value);
        
        Assert.Equal(fighter.Id, updateFighterCommand.Id);
        Assert.Equal(fighter.TournamentId, updateFighterCommand.TournamentId);
        Assert.Equal(fighter.BeltId, updateFighterCommand.BeltId);
        Assert.Equal(fighter.TrainerId, updateFighterCommand.TrainerId);
        Assert.Equal(fighter.WeightCategorieId, updateFighterCommand.WeightCategorieId);
        Assert.Equal(fighter.Name, updateFighterCommand.Name);
        Assert.Equal(fighter.Surname, updateFighterCommand.Surname);
        Assert.Equal(fighter.BirthDate, updateFighterCommand.BirthDate);
        Assert.Equal(fighter.Country, updateFighterCommand.Country);
        Assert.Equal(fighter.City, updateFighterCommand.City);
        Assert.Equal(fighter.Gender.MapToString(), updateFighterCommand.Gender);
    }
    
    [Fact]
    public async Task FighterBracketId_Must_BeSet()
    {
        var bracketId = Guid.NewGuid();
        
        var fighter = await FighterBuilder.BuildAsync(Guid.NewGuid());
        fighter.SetBracket(bracketId);
       
        Assert.Equal(fighter.BracketId, bracketId);
    }
}