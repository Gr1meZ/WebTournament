using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Select2.Queries;

public class Select2BeltQuery :  BaseSelect2Request<Domain.Objects.Belt.Belt>, IQuery<Select2Response>
{
    internal override async Task<Select2Response> GetPagedResult(IQueryable<Domain.Objects.Belt.Belt> belts, CancellationToken cancellationToken)
    {
        var total = await belts.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(Search))
        {
            belts = belts.Where(x => x.ShortName.ToLower().Contains(Search.ToLower()) 
                                     || x.BeltNumber.ToString().Contains(Search.ToLower()));
        }

        if (PageSize != -1)
            belts = belts.Skip(Skip).Take(PageSize);

        var data = await belts.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = $"{x.BeltNumber} {x.ShortName}"
            })
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }
}