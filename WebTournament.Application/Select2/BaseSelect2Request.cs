using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Select2;

public abstract class BaseSelect2Request<TEntity> where TEntity : Entity
{
    public string Search { get; set; }
    public int Skip { get; set; }
    public int PageSize { get; set; }

    internal abstract Task<Select2Response> GetPagedResult(IQueryable<TEntity> entities,
        CancellationToken cancellationToken);


}