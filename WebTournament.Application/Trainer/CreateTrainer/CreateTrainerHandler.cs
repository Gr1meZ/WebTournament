using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Trainer.CreateTrainer;

public class CreateTrainerHandler : ICommandHandler<CreateTrainerCommand>
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTrainerHandler(ITrainerRepository trainerRepository, IUnitOfWork unitOfWork)
    {
        _trainerRepository = trainerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await Domain.Objects.Trainer.Trainer.CreateAsync(Guid.NewGuid(), request.Name, request.Surname, request.Patronymic,
            request.Phone, request.ClubId.Value, _trainerRepository);
       
        await _trainerRepository.AddAsync(trainer);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}