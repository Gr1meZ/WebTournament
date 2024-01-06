using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Select2.Queries;

public class Select2TrainersQuery : BaseSelect2Request<Domain.Objects.Trainer.Trainer>, IQuery<Select2Response>
{
    internal override async Task<Select2Response> GetPagedResult(IQueryable<Domain.Objects.Trainer.Trainer> trainers, CancellationToken cancellationToken)
    {
        var total = await trainers.CountAsync(cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(Search))
        {
            trainers = trainers.Where(x => x.Name.ToLower().Contains(Search.ToLower()) ||
                                         x.Surname.ToLower().Contains(Search.ToLower()) ||
                                         x.Patronymic.ToLower().Contains(Search.ToLower()));
        }

        if (PageSize != -1)
            trainers = trainers.Skip(Skip).Take(PageSize);

        var data = trainers.Select(x => new Select2Data()
            {
                Id = x.Id,
                Name = $"{x.Surname} {x.Name[0]}.{x.Patronymic}"
            })
            .ToArray();

        return new Select2Response()
        {
            Data = data,
            Total = total
        };
    }
}