using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Tournament;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Tournament.UpdateTournament;

public class UpdateTournamentHandler : ICommandHandler<UpdateTournamentCommand>
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTournamentHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork)
    {
        _tournamentRepository = tournamentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(request.Id);
        tournament.Change(request.Name, request.StartDate, request.Address);
        
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}