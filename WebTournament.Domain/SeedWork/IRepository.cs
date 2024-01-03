namespace WebTournament.Domain.SeedWork;

public interface IRepository<TEntity> : IDisposable
{
    Task AddAsync(TEntity obj);

    Task<TEntity> GetByIdAsync(Guid id);

    IQueryable<TEntity> GetAll();
    
    void Update(TEntity obj);

    void Remove(TEntity obj);
    void RemoveAll(IQueryable<TEntity> objQuery);

}