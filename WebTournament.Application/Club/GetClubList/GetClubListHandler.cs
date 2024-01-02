using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Club;

namespace WebTournament.Application.Club.GetClubList;

public class GetClubListHandler : IQueryHandler<GetClubListQuery, PagedResponse<ClubDto[]>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMapper _mapper;

    public GetClubListHandler(IClubRepository clubRepository, IMapper mapper)
    {
        _clubRepository = clubRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<ClubDto[]>> Handle(GetClubListQuery request, CancellationToken cancellationToken)
    {
        var clubsQuery = _clubRepository.GetAll();

        var lowerQ = request.Search.ToLower();
        if (!string.IsNullOrWhiteSpace(lowerQ))
        {
            clubsQuery = (lowerQ.Split(' ')).Aggregate(clubsQuery, (current, searchWord) =>
                current.Where(f => f.Name.ToLower().Contains(searchWord.ToLower())));
        }

        if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
        {
            clubsQuery = request.OrderColumn switch
            {
                "name" => (request.OrderDir.Equals("asc"))
                    ? clubsQuery.OrderBy(o => o.Name)
                    : clubsQuery.OrderByDescending(o => o.Name),
                _ => (request.OrderDir.Equals("asc"))
                    ? clubsQuery.OrderBy(o => o.Id)
                    : clubsQuery.OrderByDescending(o => o.Id)
            };
        }

        var totalItemCount = clubsQuery.Count();

        clubsQuery = clubsQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

        var dbItems = await clubsQuery
            .Select(x => _mapper.Map<ClubDto>(x))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedResponse<ClubDto[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
    }
}