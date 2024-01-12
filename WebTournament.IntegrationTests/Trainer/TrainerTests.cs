using CustomExceptionsLibrary;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Trainer;
using WebTournament.Application.Trainer.CreateTrainer;
using WebTournament.Application.Trainer.GetTrainer;
using WebTournament.Application.Trainer.GetTrainerList;
using WebTournament.Application.Trainer.RemoveTrainer;
using WebTournament.Application.Trainer.UpdateTrainer;

namespace WebTournament.IntegrationTests.Trainer;

public class TrainerTests : BaseIntegrationTest
{
    public TrainerTests(WebApplicationFactory factory) : base(factory)
    {
    }
      [Fact]
    public async Task Trainer_Must_BeCreated()
    {
        var createTrainerCommand = await CreateTrainerCommandAsync();
        
        await Sender.Send(createTrainerCommand);
        
        var trainer = await DbContext.Trainers.FirstOrDefaultAsync(x => x.Name == createTrainerCommand.Name);
        
        Assert.NotNull(trainer);
    }
    
    [Fact]
    public async Task Trainer_AlreadyExists_ThrowsException()
    {
        var createTrainerCommand = await CreateTrainerCommandAsync();
        
        await Sender.Send(createTrainerCommand);
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(createTrainerCommand));
    }
    
    [Fact]
    public async Task GetTrainer_IsValid()
    {
        var trainerId = await DbContext.Trainers.Select(x => x.Id).FirstOrDefaultAsync();
        var getTrainerQuery = new GetTrainerQuery(trainerId);
        
        var trainerResponse = await Sender.Send(getTrainerQuery);
        
        Assert.NotNull(trainerResponse);
    }
    
    [Fact]
    public async Task GetTrainer_NotFound_ThrowsException()
    {
        var getTrainerQuery = new GetTrainerQuery(Guid.NewGuid());
        
        await Assert.ThrowsAsync<ValidationException>(() => Sender.Send(getTrainerQuery));
    }
    
    [Fact]
    public async Task GetTrainerList_Must_ReturnValidPagedResponse()
    {
        var clubId = await GetRandomClubIdAsync();
        await Sender.Send(new CreateTrainerCommand() { Name = "ЮношКлуб111", Surname = Guid.NewGuid().ToString(),
            Patronymic = Guid.NewGuid().ToString(), Phone = Guid.NewGuid().ToString(), ClubId = clubId});
        await Sender.Send(new CreateTrainerCommand() { Name = "ЮношКлуб1111", Surname = Guid.NewGuid().ToString(),
            Patronymic = Guid.NewGuid().ToString(), Phone = Guid.NewGuid().ToString(), ClubId = clubId});
        await Sender.Send(new CreateTrainerCommand() { Name = "ЮношКлуб11111", Surname = Guid.NewGuid().ToString(),
            Patronymic = Guid.NewGuid().ToString(), Phone = Guid.NewGuid().ToString(), ClubId = clubId});

        var getTrainerListBySearch = new GetTrainerListQuery() { Search = "Клуб" };
        var getTrainerListByOrder = new GetTrainerListQuery() {OrderColumn = "name", OrderDir = "desc", Search = ""};

        var searchResponse = await Sender.Send(getTrainerListBySearch);
        var orderColumnResponse = await Sender.Send(getTrainerListByOrder);

        var dataBySearch = await DbContext.Trainers.Where(x => x.Name.Contains(getTrainerListBySearch.Search)).ToListAsync();
        var dataByOrder = await DbContext.Trainers.OrderByDescending(x => x.Name).ToListAsync();
        
        var expectedSearch = dataBySearch.Select(trainer => Mapper.Map<TrainerResponse>(trainer)).ToArray();
        var expectedOrder = dataByOrder.Select(trainer => Mapper.Map<TrainerResponse>(trainer)).ToArray();
        
        Assert.Equal(searchResponse.Metadata.TotalItemCount, expectedSearch.Length);
        Assert.Equal(orderColumnResponse.Metadata.TotalItemCount, expectedOrder.Length);

    }
    
    [Fact]
    public async Task Trainer_Must_BeRemoved()
    {
        var createTrainerCommand = await CreateTrainerCommandAsync();
        
        await Sender.Send(createTrainerCommand);

        var trainerId =  await DbContext.Trainers
            .Where(x => x.Name == createTrainerCommand.Name)
            .Select(x => x.Id).FirstOrDefaultAsync();

        var removeCommand = new RemoveTrainerCommand(trainerId);

        await Sender.Send(removeCommand);

        var isDeleted = !await DbContext.Trainers.AnyAsync(x => x.Id == trainerId);
        
        Assert.True(isDeleted);
    }
    
    [Fact]
    public async Task Trainer_Must_BeUpdated()
    {
        var clubId = await GetRandomClubIdAsync();
        var trainerId = await DbContext.Trainers.Select(x => x.Id).FirstOrDefaultAsync();
        var updateCommand = new UpdateTrainerCommand() {Id = trainerId, Name = "ЮношКлуб111111", Surname = Guid.NewGuid().ToString(),
            Patronymic = Guid.NewGuid().ToString(), Phone = Guid.NewGuid().ToString(), ClubId = clubId};
        
        await Sender.Send(updateCommand);

        var updatedTrainer = await DbContext.Trainers.FindAsync(trainerId);
        
        
        Assert.Equal(updateCommand.Id, updatedTrainer.Id);
        Assert.Equal(updateCommand.Name, updatedTrainer.Name);
        Assert.Equal(updateCommand.Surname, updatedTrainer.Surname);
        Assert.Equal(updateCommand.Patronymic, updatedTrainer.Patronymic);
        Assert.Equal(updateCommand.Phone, updatedTrainer.Phone);
        Assert.Equal(updateCommand.ClubId, updatedTrainer.ClubId);

    }
}