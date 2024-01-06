using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Tournament;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Tournament.RemoveTournament;

public class RemoveTournamentHandler : ICommandHandler<RemoveTournamentCommand>
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveTournamentHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork)
    {
        _tournamentRepository = tournamentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(request.Id);

        _tournamentRepository.Remove(tournament);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}