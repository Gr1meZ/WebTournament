using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Tournament;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Tournament.CreateTournament;

public class CreateTournamentHandler : ICommandHandler<CreateTournamentCommand>
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateTournamentHandler(ITournamentRepository tournamentRepository, IUnitOfWork unitOfWork)
    {
        _tournamentRepository = tournamentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateTournamentCommand request, CancellationToken cancellationToken)
    {
        var tournament = await Domain.Objects.Tournament.Tournament.CreateAsync(Guid.NewGuid(), request.Name, request.StartDate,
            request.Address, _tournamentRepository);
        
        await _tournamentRepository.AddAsync(tournament);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}