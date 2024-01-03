using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Fighter.RemoveFighter;

public class RemoveFighterHandler : ICommandHandler<RemoveFighterCommand>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFighterHandler(IFighterRepository fighterRepository, IUnitOfWork unitOfWork)
    {
        _fighterRepository = fighterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveFighterCommand request, CancellationToken cancellationToken)
    {
        var fighter = await _fighterRepository.GetByIdAsync(request.Id);
        
        _fighterRepository.Remove(fighter);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}