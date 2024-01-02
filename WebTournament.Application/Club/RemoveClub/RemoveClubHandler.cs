using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Club;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Club.RemoveClub;

public class RemoveClubHandler : ICommandHandler<RemoveClubCommand>
{
    private readonly IClubRepository _clubRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveClubHandler(IClubRepository clubRepository, IUnitOfWork unitOfWork)
    {
        _clubRepository = clubRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveClubCommand request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetByIdAsync(request.Id);
        
        _clubRepository.Remove(club);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}