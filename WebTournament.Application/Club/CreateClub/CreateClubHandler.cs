using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Club;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Club.CreateClub;

public class CreateClubHandler : ICommandHandler<CreateClubCommand>
{
    private readonly IClubRepository _clubRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClubHandler(IClubRepository clubRepository, IUnitOfWork unitOfWork)
    {
        _clubRepository = clubRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateClubCommand request, CancellationToken cancellationToken)
    {
        var club = await Domain.Objects.Club.Club.CreateAsync(Guid.NewGuid(), request.Name, _clubRepository);
        
        await _clubRepository.AddAsync(club);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
    
}