using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.Belt.CreateBelt;
using WebTournament.Application.Bracket.GenerateBracket;
using WebTournament.Application.Club.CreateClub;
using WebTournament.Application.Fighter.CreateFighter;
using WebTournament.Application.Tournament.CreateTournament;
using WebTournament.Application.Trainer.CreateTrainer;
using WebTournament.Application.WeightCategorie.CreateWeightCategorie;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Extensions;
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
    
    
    //Domain builders
    protected async Task<Guid> GetRandomAgeGroupIdAsync()
    {
        var ageGroupCmd = new CreateAgeGroupCommand()
            { Name = Guid.NewGuid().ToString(), MinAge = new Random().Next(0, 10), MaxAge = new Random().Next(11, 18) };
        
        await Sender.Send(ageGroupCmd);
        
        return await DbContext.AgeGroups
            .Where(x => x.Name == ageGroupCmd.Name)
            .Select(x => x.Id).FirstOrDefaultAsync();
    }
    
     protected async Task<Guid> GetRandomTournamentIdAsync()
     {
         var tournamentCmd = CreateTournamentCommand();
        
        await Sender.Send(tournamentCmd);
        
        return await DbContext.Tournaments
            .Where(x => x.Name == tournamentCmd.Name && x.StartDate == tournamentCmd.StartDate)
            .Select(x => x.Id).FirstOrDefaultAsync();
    }
     
     protected async Task<Guid> GetRandomFighterIdAsync()
     {
         var fighterCommand = await CreateFighterCommandAsync();
        
         await Sender.Send(fighterCommand);
        
         return await DbContext.Fighters
             .Where(x => x.Name == fighterCommand.Name)
             .Select(x => x.Id).FirstOrDefaultAsync();
     }
     
    protected async Task<Guid> CreateRandomBeltIdAsync()
    {
        var beltCmd = CreateBeltCommand();
        
        await Sender.Send(beltCmd);
        
        return await DbContext.Belts
            .Where(x => x.BeltNumber == beltCmd.BeltNumber && x.ShortName == beltCmd.ShortName)
            .Select(x => x.Id).FirstOrDefaultAsync();
    }
    
    protected async Task<Guid> CreateRandomTrainerIdAsync()
    {
        var trainerCmd = await CreateTrainerCommandAsync();
        
        await Sender.Send(trainerCmd);
        
        return await DbContext.Trainers
            .Where(x => x.Name == trainerCmd.Name && x.Surname == trainerCmd.Surname
                                                  && x.ClubId == trainerCmd.ClubId && x.Phone == trainerCmd.Phone)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }
    
    protected async Task<Guid> CreateRandomWeightCategorieIdAsync()
    {
        var weightCategorieCommand = await CreateWeightCategorieCommandAsync();
        
        await Sender.Send(weightCategorieCommand);
        
        return await DbContext.WeightCategories
            .Where(x => x.Gender == GenderExtension.ParseEnum(weightCategorieCommand.Gender) && x.MaxWeight == weightCategorieCommand.MaxWeight
                                                  && x.AgeGroupId == weightCategorieCommand.AgeGroupId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }
    
    protected async Task<Guid> GetRandomClubIdAsync()
    {
        var clubCommand = CreateClubCommand();
        
        await Sender.Send(clubCommand);

        return await DbContext.Clubs
            .Where(x => x.Name == clubCommand.Name)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }
    
    //Commands builders
    protected CreateTournamentCommand CreateTournamentCommand() => new CreateTournamentCommand()
         { Name = Guid.NewGuid().ToString(), Address = Guid.NewGuid().ToString(), StartDate = DateTime.UtcNow.AddDays(new Random().Next(0, 50)) };
     
     protected CreateBeltCommand CreateBeltCommand() => new CreateBeltCommand()
    { BeltNumber = new Random().Next(0, 10), ShortName = Guid.NewGuid().ToString(), FullName = Guid.NewGuid().ToString()};
  
    
     protected async Task<CreateTrainerCommand> CreateTrainerCommandAsync()
    {
        await Sender.Send(new CreateClubCommand() { Name = Guid.NewGuid().ToString() });
        var clubId = await DbContext.Clubs.Select(x => x.Id).FirstOrDefaultAsync();
        
        return new CreateTrainerCommand()
            { 
                Name = Guid.NewGuid().ToString(),
                Surname  = Guid.NewGuid().ToString(),
                Patronymic = Guid.NewGuid().ToString(), 
                Phone  = Guid.NewGuid().ToString(),
                ClubId = clubId
            };
    }
    
    protected async Task<CreateWeightCategorieCommand> CreateWeightCategorieCommandAsync()
    {
        var ageGroupId = await DbContext.AgeGroups.Select(x => x.Id).FirstOrDefaultAsync();
        if (ageGroupId == Guid.Empty)
        {
            var ageGroupCmd = new CreateAgeGroupCommand(){Name = "Test", MaxAge = new Random().Next(20), MinAge = new Random().Next(50)};
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

        return weightCategorieCommand;
    }

    protected async Task<CreateFighterCommand> CreateFighterCommandAsync()
    {
        return  new CreateFighterCommand()
        {
            TournamentId =  await GetRandomTournamentIdAsync(),
            BeltId = await CreateRandomBeltIdAsync(), 
            TrainerId = await CreateRandomTrainerIdAsync(),
            WeightCategorieId = await CreateRandomWeightCategorieIdAsync(),
            Name = Guid.NewGuid().ToString(), 
            Surname = Guid.NewGuid().ToString(), 
            BirthDate = DateTime.UtcNow.AddYears(-10), 
            Country = Guid.NewGuid().ToString(),
            City = Guid.NewGuid().ToString(), 
            Gender = Gender.Male.MapToString()
        };
    }
    
    protected async Task<CreateFighterCommand> CreateFighterCommandAsync(Guid tournamentId, Guid weightCategorieId, Guid beltId)
    {
        return  new CreateFighterCommand()
        {
            TournamentId =  tournamentId,
            BeltId = beltId, 
            TrainerId = await CreateRandomTrainerIdAsync(),
            WeightCategorieId = weightCategorieId,
            Name = Guid.NewGuid().ToString(), 
            Surname = Guid.NewGuid().ToString(), 
            BirthDate = DateTime.UtcNow.AddYears(-10), 
            Country = Guid.NewGuid().ToString(),
            City = Guid.NewGuid().ToString(), 
            Gender = Gender.Male.MapToString()
        };
    }
    protected CreateClubCommand CreateClubCommand()
    {
        return  new CreateClubCommand()
        {
            Name = Guid.NewGuid().ToString(), 
        };
    }

    protected async Task<GenerateBracketCommand> CreateBracketCommand()
    {
        var tournamentId = await GetRandomTournamentIdAsync();
        var weightCategorieId = await CreateRandomWeightCategorieIdAsync();
        var beltId =  await CreateRandomBeltIdAsync();
        for (var i = 0; i < 10; i++)
        {
            var command = await CreateFighterCommandAsync(tournamentId, weightCategorieId, beltId);
            await Sender.Send(command);
        }
        var ageGroupId = await DbContext.Fighters
            .Where(x => x.TournamentId == tournamentId)
            .Select(x => x.WeightCategorie.AgeGroupId)
            .FirstOrDefaultAsync();
        
        var anyBeltId = await DbContext.Fighters
            .Where(x => x.TournamentId == tournamentId)
            .Select(x => x.BeltId).FirstOrDefaultAsync();
        
        return new GenerateBracketCommand()
        {
            TournamentId = tournamentId,
            AgeGroupId = ageGroupId,
            Division = new[] { anyBeltId },
            Id = weightCategorieId
        };
    }
}