using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;
using WebTournament.Domain.Objects.Tournament;

namespace WebTournament.Application.Tournament.GetTournamentList;

public class GetTournamentListHandler : IQueryHandler<GetTournamentListQuery, PagedResponse<TournamentResponse[]>>
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IMapper _mapper;

    public GetTournamentListHandler(IMapper mapper, ITournamentRepository tournamentRepository)
    {
        _mapper = mapper;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<PagedResponse<TournamentResponse[]>> Handle(GetTournamentListQuery request, CancellationToken cancellationToken)
    {
        var tournaments = _tournamentRepository.GetAll();

        var tournamentSpecificationResult = await new TournamentSpecification(tournaments, request.Search,
            request.OrderColumn, request.OrderDir)
            .GetSpecificationResultAsync(request.PageNumber, request.PageSize, cancellationToken);
        
        var dbItems = await tournamentSpecificationResult.Entities
            .Select(x => _mapper.Map<TournamentResponse>(x))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedResponse<TournamentResponse[]>(dbItems, tournamentSpecificationResult.Total, request.PageNumber, request.PageSize);
    }
}