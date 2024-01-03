using WebTournament.Application.Configuration.Commands;
using WebTournament.Application.Fighter.RemoveFighter;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Fighter.RemoveAllFighters;

public class RemoveAllFightersHandler : ICommandHandler<RemoveAllFightersCommand>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveAllFightersHandler(IFighterRepository fighterRepository, IUnitOfWork unitOfWork)
    {
        _fighterRepository = fighterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveAllFightersCommand request, CancellationToken cancellationToken)
    {
        var fighters =  _fighterRepository.GetAllByTournamentId(request.Id);
        
        _fighterRepository.RemoveAll(fighters);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}