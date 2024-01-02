using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.WeightCategorie;

namespace WebTournament.Application.WeightCategorie.GetWeightCategorieList;

public class GetWeightCategorieListHandler : IQueryHandler<GetWeightCategorieListQuery, PagedResponse<WeightCategorieDto[]>>
{
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IMapper _mapper;

    public GetWeightCategorieListHandler(IWeightCategorieRepository weightCategorieRepository, IMapper mapper)
    {
        _weightCategorieRepository = weightCategorieRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<WeightCategorieDto[]>> Handle(GetWeightCategorieListQuery query,
        CancellationToken cancellationToken)
    {
        var weightCategorieQuery = _weightCategorieRepository.GetAll();

        var lowerQ = query.Search.ToLower();
        if (!string.IsNullOrWhiteSpace(lowerQ))
        {
            weightCategorieQuery = (lowerQ.Split(' ')).Aggregate(weightCategorieQuery, (current, searchWord) =>
                current.Where(f =>
                    f.AgeGroup.Name.ToLower().Contains(searchWord.ToLower()) ||
                    f.MaxWeight.ToString().ToLower().Contains(searchWord.ToLower()) ||
                    f.WeightName.ToLower().Contains(searchWord.ToLower())
                ));
        }

        if (!string.IsNullOrWhiteSpace(query.OrderColumn) && !string.IsNullOrWhiteSpace(query.OrderDir))
        {
            weightCategorieQuery = query.OrderColumn switch
            {
                "weightName" => (query.OrderDir.Equals("asc"))
                    ? weightCategorieQuery.OrderBy(o => o.WeightName)
                    : weightCategorieQuery.OrderByDescending(o => o.WeightName),
                "maxWeight" => (query.OrderDir.Equals("asc"))
                    ? weightCategorieQuery.OrderBy(o => o.MaxWeight)
                    : weightCategorieQuery.OrderByDescending(o => o.MaxWeight),
                "ageGroupName" => (query.OrderDir.Equals("asc"))
                    ? weightCategorieQuery.OrderBy(o => o.AgeGroup.Name)
                    : weightCategorieQuery.OrderByDescending(o => o.AgeGroup.Name),
                "gender" => (query.OrderDir.Equals("asc"))
                    ? weightCategorieQuery.OrderBy(o => o.Gender)
                    : weightCategorieQuery.OrderByDescending(o => o.Gender),
                _ => (query.OrderDir.Equals("asc"))
                    ? weightCategorieQuery.OrderBy(o => o.Id)
                    : weightCategorieQuery.OrderByDescending(o => o.Id)
            };
        }

        var totalItemCount = await weightCategorieQuery.CountAsync(cancellationToken: cancellationToken);

        weightCategorieQuery = weightCategorieQuery.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);

        var dbItems = await weightCategorieQuery
            .Select(x => _mapper.Map<WeightCategorieDto>(x))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedResponse<WeightCategorieDto[]>(dbItems, totalItemCount, query.PageNumber, query.PageSize);
    }
}