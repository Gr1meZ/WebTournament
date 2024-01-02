using WebTournament.Application.Configuration.Commands;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Club;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Club.UpdateClub;

public class UpdateClubHandler : ICommandHandler<UpdateClubCommand>
{
    private readonly IClubRepository _clubRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClubHandler(IClubRepository clubRepository, IUnitOfWork unitOfWork)
    {
        _clubRepository = clubRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateClubCommand request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetByIdAsync(request.Id);
        
        club.Change(request.Name);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}