using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;
using WebTournament.Domain.Objects.Club;

namespace WebTournament.Application.Club.GetClubList;

public class GetClubListHandler : IQueryHandler<GetClubListQuery, PagedResponse<ClubResponse[]>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMapper _mapper;

    public GetClubListHandler(IClubRepository clubRepository, IMapper mapper)
    {
        _clubRepository = clubRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<ClubResponse[]>> Handle(GetClubListQuery request, CancellationToken cancellationToken)
    {
        var clubsQuery = _clubRepository.GetAll();
        
        var clubSpecificationResult = await 
            new ClubSpecification(clubsQuery, request.Search, request.OrderColumn, request.OrderDir)
                .GetSpecificationResult(request.PageNumber, request.PageSize, cancellationToken);
        
        var dbItems = await clubSpecificationResult.Entities
            .Select(x => _mapper.Map<ClubResponse>(x))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedResponse<ClubResponse[]>(dbItems, clubSpecificationResult.Total, request.PageNumber, request.PageSize);
    }
}