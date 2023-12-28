using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Common;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.Select2.Queries;
using WebTournament.Domain.Objects.AgeGroup;

namespace WebTournament.Application.Select2;

public class Select2Handler : IQueryHandler<Select2AgeGroupsQuery, Select2Response>
{
    private readonly IAgeGroupRepository _ageGroupRepository;

    public Select2Handler(IAgeGroupRepository ageGroupRepository)
    {
        _ageGroupRepository = ageGroupRepository;
    }

    public async Task<Select2Response> Handle(Select2AgeGroupsQuery request, CancellationToken cancellationToken)
    {
        var ageGroups = _ageGroupRepository.GetAll();

        var dbQuery = ageGroups;
        var total = await ageGroups.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            dbQuery = dbQuery.Where(x => x.Name.ToLower().Contains(request.Search.ToLower()));
        }

        if (request.PageSize != -1)
            dbQuery = dbQuery.Skip(request.Skip).Take(request.PageSize);

        var data = await dbQuery.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }
}