using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;
using WebTournament.Domain.Objects.Fighter;

namespace WebTournament.Application.Fighter.GetFighterList;

public class GetFighterListHandler : IQueryHandler<GetFighterListQuery, PagedResponse<FighterResponse[]>>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IMapper _mapper;

    public GetFighterListHandler(IFighterRepository fighterRepository, IMapper mapper)
    {
        _fighterRepository = fighterRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<FighterResponse[]>> Handle(GetFighterListQuery request,
        CancellationToken cancellationToken)
    {
        var fightersQuery = _fighterRepository.GetAll(request.TournamentId);
        
        var fighterSpecificationResult = await
            new FighterSpecification(fightersQuery, request.Search, request.OrderColumn, request.OrderDir)
                .GetSpecificationResult(request.PageNumber, request.PageSize, cancellationToken);

        var dbItems = await fighterSpecificationResult.Entities.Select(x => _mapper.Map<FighterResponse>(x))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedResponse<FighterResponse[]>(dbItems, fighterSpecificationResult.Total, request.PageNumber, request.PageSize);
    }
}