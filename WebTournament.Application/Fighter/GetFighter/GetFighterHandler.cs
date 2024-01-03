using AutoMapper;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Fighter;

namespace WebTournament.Application.Fighter.GetFighter;

public class GetFighterHandler : IQueryHandler<GetFighterQuery, FighterDto>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IMapper _mapper;

    public GetFighterHandler(IFighterRepository fighterRepository, IMapper mapper)
    {
        _fighterRepository = fighterRepository;
        _mapper = mapper;
    }

    public async Task<FighterDto> Handle(GetFighterQuery request, CancellationToken cancellationToken)
    {
        var fighter = await _fighterRepository.GetByIdAsync(request.Id);
        return _mapper.Map<FighterDto>(fighter);
    }
}