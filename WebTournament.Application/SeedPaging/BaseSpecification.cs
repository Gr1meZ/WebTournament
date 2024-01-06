using Microsoft.EntityFrameworkCore;
using WebTournament.Application.AgeGroup.GetAgeGroupList;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.SeedPaging;

public abstract class BaseSpecification<TEntity> where TEntity : Entity
{
    protected string Search { get; set;}
    protected string OrderColumn { get;  set;}
    protected string OrderDir { get;  set;}
    protected IQueryable<TEntity> Entities { get;  set; }
    protected int Total { get; set; }

    protected BaseSpecification(IQueryable<TEntity> entities, string search, string orderColumn, string orderDir)
    {
        Entities = entities;
        Search = search.ToLower();
        OrderColumn = orderColumn;
        OrderDir = orderDir;
    }

    protected abstract void SearchData();
    protected abstract void OrderData();
    protected virtual async Task<int> GetTotalAsync(CancellationToken cancellationToken) =>
         Total = await Entities.CountAsync(cancellationToken);

    public virtual async Task<SpecificationResult<TEntity>> GetSpecificationResultAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var total = await GetTotalAsync(cancellationToken);
        Entities = Entities.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return new SpecificationResult<TEntity>(Entities, total);
    }
    
}