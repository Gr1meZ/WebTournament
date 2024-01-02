using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Trainer.UpdateTrainer;

public class UpdateTrainerHandler : ICommandHandler<UpdateTrainerCommand>
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateTrainerHandler(ITrainerRepository trainerRepository, IUnitOfWork unitOfWork)
    {
        _trainerRepository = trainerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await _trainerRepository.GetByIdAsync(request.Id);
        trainer.Change(request.Name, request.Surname, request.Patronymic, request.Phone, request.ClubId.Value);

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}