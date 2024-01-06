using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.BracketWinner;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Fighter.RemoveAllFighters;

public class RemoveAllFightersHandler : ICommandHandler<RemoveAllFightersCommand>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IBracketWinnerRepository _bracketWinnerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveAllFightersHandler(IFighterRepository fighterRepository, IUnitOfWork unitOfWork, IBracketWinnerRepository bracketWinnerRepository)
    {
        _fighterRepository = fighterRepository;
        _unitOfWork = unitOfWork;
        _bracketWinnerRepository = bracketWinnerRepository;
    }

    public async Task Handle(RemoveAllFightersCommand request, CancellationToken cancellationToken)
    {
        var fighters =  _fighterRepository.GetAllByTournamentId(request.Id);
        var bracketWinners = _bracketWinnerRepository.GetAll().Where(x => x.Bracket.TournamentId == request.Id);
       
        _fighterRepository.RemoveAll(fighters);
        _bracketWinnerRepository.RemoveAll(bracketWinners);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}