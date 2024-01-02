using AutoMapper;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Club;

namespace WebTournament.Application.Club.GetClub;

public class GetClubHandler : IQueryHandler<GetClubQuery, ClubDto>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMapper _mapper;
    
    public GetClubHandler(IClubRepository clubRepository, IMapper mapper)
    {
        _clubRepository = clubRepository;
        _mapper = mapper;
    }

    public async Task<ClubDto> Handle(GetClubQuery request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetByIdAsync(request.Id);
        return _mapper.Map<ClubDto>(club);
    }
}