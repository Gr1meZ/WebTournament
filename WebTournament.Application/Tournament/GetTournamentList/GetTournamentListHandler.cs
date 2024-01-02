using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Tournament;

namespace WebTournament.Application.Tournament.GetTournamentList;

public class GetTournamentListHandler : IQueryHandler<GetTournamentListQuery, PagedResponse<TournamentDto[]>>
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IMapper _mapper;

    public GetTournamentListHandler(IMapper mapper, ITournamentRepository tournamentRepository)
    {
        _mapper = mapper;
        _tournamentRepository = tournamentRepository;
    }

    public async Task<PagedResponse<TournamentDto[]>> Handle(GetTournamentListQuery request, CancellationToken cancellationToken)
    {
        var dbQuery = _tournamentRepository.GetAll();

            // searching
            var lowerQ = request.Search.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.StartDate.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.Address.ToLower().Contains(searchWord.ToLower())
                    ));
            }

            // sorting
            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
            {
                dbQuery = request.OrderColumn switch
                {
                    "name" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Name)
                        : dbQuery.OrderByDescending(o => o.Name),
                    "startDate" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.StartDate)
                    : dbQuery.OrderByDescending(o => o.StartDate),
                    "address" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Address)
                        : dbQuery.OrderByDescending(o => o.Address),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            // total count
            var totalItemCount = await dbQuery.CountAsync(cancellationToken: cancellationToken);

            // paging
            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems = await dbQuery
                .Select(x => _mapper.Map<TournamentDto>(x))
                .ToArrayAsync(cancellationToken: cancellationToken);

            return new PagedResponse<TournamentDto[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
    }
}