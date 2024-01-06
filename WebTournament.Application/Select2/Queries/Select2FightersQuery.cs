
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Select2.Queries;

public class Select2FightersQuery : BaseSelect2Request<Domain.Objects.Fighter.Fighter>, IQuery<Select2Response>
{
    public Guid Id { get; set; }

    internal override async Task<Select2Response> GetPagedResult(IQueryable<Domain.Objects.Fighter.Fighter> fighters, CancellationToken cancellationToken)
    {
        var total = await fighters.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(Search))
        {
            fighters = fighters.Where(x => x.Surname.ToLower().Contains(Search.ToLower()));
        }

        if (PageSize != -1)
            fighters = fighters.Skip(Skip).Take(PageSize);

        var data = fighters.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = $"{x.Surname}"
            })
            .ToArray();

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }
}