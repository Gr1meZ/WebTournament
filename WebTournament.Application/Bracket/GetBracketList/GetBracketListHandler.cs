using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.Objects.Bracket;

namespace WebTournament.Application.Bracket.GetBracketList;

public class GetBracketListHandler : IQueryHandler<GetBracketListQuery, PagedResponse<BracketDto[]>>
{
    private readonly IBracketRepository _bracketRepository;
    private readonly IBeltRepository _beltRepository;
    public GetBracketListHandler(IBracketRepository bracketRepository, IBeltRepository beltRepository)
    {
        _bracketRepository = bracketRepository;
        _beltRepository = beltRepository;
    }

    public async Task<PagedResponse<BracketDto[]>> Handle(GetBracketListQuery request, CancellationToken cancellationToken)
    {
        var bracketQuery = _bracketRepository.GetAll(request.TournamentId);
        
        var lowerQ = request.Search.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                bracketQuery = lowerQ.Split(' ').Aggregate(bracketQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Tournament.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.WeightName.ToLower().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.MaxWeight.ToString().Contains(searchWord.ToLower()) ||
                        f.WeightCategorie.AgeGroup.Name.ToLower().Contains(searchWord.ToLower())
                    ));
            }

            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
            {
                bracketQuery = request.OrderColumn switch
                {
                    "divisionName" => (request.OrderDir.Equals("asc"))
                        ? bracketQuery.OrderBy(o => o.Division)
                        : bracketQuery.OrderByDescending(o => o.Division),
                    "categoriesName" => (request.OrderDir.Equals("asc"))
                    ? bracketQuery.OrderBy(o => o.WeightCategorie.AgeGroup.MinAge)
                    : bracketQuery.OrderByDescending(o => o.WeightCategorie.AgeGroup.MinAge),
                    "maxWeight" => (request.OrderDir.Equals("asc"))
                        ? bracketQuery.OrderBy(o => o.WeightCategorie.MaxWeight)
                        : bracketQuery.OrderByDescending(o => o.WeightCategorie.MaxWeight),
                    _ => (request.OrderDir.Equals("asc"))
                        ? bracketQuery.OrderBy(o => o.WeightCategorie.MaxWeight)
                        : bracketQuery.OrderByDescending(o => o.WeightCategorie.MaxWeight)
                };
            }

            var totalItemCount = await bracketQuery.CountAsync(cancellationToken: cancellationToken);

            bracketQuery = bracketQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);
            var dbItems = await bracketQuery.Select(x => new BracketDto()
            {
                Id = x.Id,
                DivisionName = string.Join(", ",  _beltRepository.GetAll().OrderBy(belt => belt.BeltNumber)
                    .Where(belt => x.Division.Contains(belt.Id)).Select(y => $"{y.BeltNumber} {y.ShortName}")),
                CategoriesName = $"{x.WeightCategorie.AgeGroup.Name} - {x.WeightCategorie.WeightName}",
                MaxWeight = x.WeightCategorie.MaxWeight
            }).ToArrayAsync(cancellationToken: cancellationToken);

            return new PagedResponse<BracketDto[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
    }
}