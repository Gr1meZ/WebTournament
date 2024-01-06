using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.SeedPaging;

public class SpecificationResult<TEntity> where TEntity : Entity
{
    public SpecificationResult(IQueryable<TEntity> entities, int total)
    {
        Entities = entities;
        Total = total;
    }

    public IQueryable<TEntity> Entities { get; }
    public int Total { get; }
}