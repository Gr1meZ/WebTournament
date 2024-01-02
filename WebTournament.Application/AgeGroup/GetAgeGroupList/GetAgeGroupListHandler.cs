using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.AgeGroup;

namespace WebTournament.Application.AgeGroup.GetAgeGroupList;

public class GetAgeGroupListHandler : IQueryHandler<GetAgeGroupListQuery, PagedResponse<AgeGroupDto[]>>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IMapper _mapper;
    public GetAgeGroupListHandler(IAgeGroupRepository ageGroupRepository, IMapper mapper)
    {
        _ageGroupRepository = ageGroupRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<AgeGroupDto[]>> Handle(GetAgeGroupListQuery request, CancellationToken cancellationToken)
    {
         var dbQuery = _ageGroupRepository.GetAll();

            // searching
            var lowerQ = request.Search.ToLower();
            if (!string.IsNullOrWhiteSpace(lowerQ))
            {
                dbQuery = (lowerQ.Split(' ')).Aggregate(dbQuery, (current, searchWord) =>
                    current.Where(f =>
                        f.Name.ToLower().Contains(searchWord.ToLower()) ||
                        f.MinAge.ToString().ToLower().Contains(searchWord.ToLower()) ||
                        f.MaxAge.ToString().ToLower().Contains(searchWord.ToLower())
                    ));
            }

            if (!string.IsNullOrWhiteSpace(request.OrderColumn) && !string.IsNullOrWhiteSpace(request.OrderDir))
            {
                dbQuery = request.OrderColumn switch
                {
                    "name" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Name)
                        : dbQuery.OrderByDescending(o => o.Name),
                    "minAge" => (request.OrderDir.Equals("asc"))
                    ? dbQuery.OrderBy(o => o.MinAge)
                    : dbQuery.OrderByDescending(o => o.MinAge),
                    "maxAge" => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.MaxAge)
                        : dbQuery.OrderByDescending(o => o.MaxAge),
                    _ => (request.OrderDir.Equals("asc"))
                        ? dbQuery.OrderBy(o => o.Id)
                        : dbQuery.OrderByDescending(o => o.Id)
                };
            }

            var totalItemCount = await dbQuery.CountAsync(cancellationToken: cancellationToken);

            dbQuery = dbQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);

            var dbItems =  await dbQuery
                .Select(x => _mapper.Map<AgeGroupDto>(x))
                .ToArrayAsync(cancellationToken: cancellationToken);

            return new PagedResponse<AgeGroupDto[]>(dbItems, totalItemCount, request.PageNumber, request.PageSize);
    }
}