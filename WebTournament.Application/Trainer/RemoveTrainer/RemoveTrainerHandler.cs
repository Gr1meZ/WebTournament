using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Trainer.RemoveTrainer;

public class RemoveTrainerHandler : ICommandHandler<RemoveTrainerCommand>
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveTrainerHandler(ITrainerRepository trainerRepository, IUnitOfWork unitOfWork)
    {
        _trainerRepository = trainerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await _trainerRepository.GetByIdAsync(request.Id);
        
        _trainerRepository.Remove(trainer);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}