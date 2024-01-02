using AutoMapper;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Tournament;

namespace WebTournament.Application.Tournament.GetTournament;

public class GetTournamentHandler : IQueryHandler<GetTournamentQuery, TournamentDto>
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IMapper _mapper;

    public GetTournamentHandler(ITournamentRepository tournamentRepository, IMapper mapper)
    {
        _tournamentRepository = tournamentRepository;
        _mapper = mapper;
    }

    public async Task<TournamentDto> Handle(GetTournamentQuery request, CancellationToken cancellationToken)
    {
        var tournament = await _tournamentRepository.GetByIdAsync(request.Id);
        return _mapper.Map<TournamentDto>(tournament);
    }
}