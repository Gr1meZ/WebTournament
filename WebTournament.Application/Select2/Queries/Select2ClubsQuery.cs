using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Select2.Queries;

public class Select2ClubsQuery : BaseSelect2Request<Domain.Objects.Club.Club>, IQuery<Select2Response>
{

    internal override async Task<Select2Response> GetPagedResult(IQueryable<Domain.Objects.Club.Club> clubs, CancellationToken cancellationToken)
    {
        var total = await clubs.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(Search))
        {
            clubs = clubs.Where(x => x.Name.ToLower().Contains(Search.ToLower()));
        }

        if (PageSize != -1)
            clubs = clubs.Skip(Skip).Take(PageSize);

        var data = clubs.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToArray();

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }
}