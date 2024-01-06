using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Select2.Queries;

public class Select2AgeGroupsQuery :  BaseSelect2Request<Domain.Objects.AgeGroup.AgeGroup>, IQuery<Select2Response>
{
    internal override async Task<Select2Response> GetPagedResult(IQueryable<Domain.Objects.AgeGroup.AgeGroup> entities, CancellationToken cancellationToken)
    {
        var total = await entities.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(Search))
        {
            entities = entities.Where(x => x.Name.ToLower().Contains(Search.ToLower()));
        }

        if (PageSize != -1)
            entities = entities.Skip(Skip).Take(PageSize);

        var data = await entities.Select(x => new Select2Data()
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