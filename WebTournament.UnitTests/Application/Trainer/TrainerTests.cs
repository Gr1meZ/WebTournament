using AutoMapper;
using Moq;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Application.Tournament;
using WebTournament.Application.Tournament.GetTournament;
using WebTournament.Application.Trainer;
using WebTournament.Application.Trainer.GetTrainer;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.UnitTests.Builders;

namespace WebTournament.UnitTests.Application.Trainer;

public class TrainerTests
{
    private readonly Mock<ITrainerRepository> _trainerRepository = new();

    [Fact]
    public async Task GetTournament_Must_BeSuccessful()
    {
        var getTrainerQuery = new GetTrainerQuery(Guid.NewGuid());
        
        var trainer = await TrainerBuilder.BuildAsync(getTrainerQuery.Id);

        _trainerRepository.Setup(method => method.GetByIdAsync(getTrainerQuery.Id))
            .ReturnsAsync(trainer);
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<ApplicationProfile>());
        var mapper = config.CreateMapper();
        var trainerResponse = mapper.Map<TrainerResponse>(trainer);
        
        Assert.Equal(trainer.Id, trainerResponse.Id);
        Assert.Equal(trainer.Name, trainerResponse.Name);
        Assert.Equal(trainer.Surname, trainerResponse.Surname);
        Assert.Equal(trainer.Patronymic, trainerResponse.Patronymic);
        Assert.Equal(trainer.Phone, trainerResponse.Phone);
        Assert.Equal(trainer.ClubId, trainerResponse.ClubId);
        
    }
}