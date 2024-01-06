using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Select2.Queries;

public class Select2WeightCategorieQuery : BaseSelect2Request<Domain.Objects.WeightCategorie.WeightCategorie>, IQuery<Select2Response>
{
    internal override async Task<Select2Response> GetPagedResult(IQueryable<Domain.Objects.WeightCategorie.WeightCategorie> weightCategories, CancellationToken cancellationToken)
    {
        var total = await weightCategories.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(Search))
        {
            weightCategories = weightCategories.Where(x => x.WeightName.ToLower().Contains(Search.ToLower()));
        }

        if (PageSize != -1)
            weightCategories = weightCategories.Skip(Skip).Take(PageSize);

        var data = weightCategories.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = x.WeightName
            })
            .ToArray();

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }
}