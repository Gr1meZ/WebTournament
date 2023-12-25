using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.Abstract;
using WebTournament.Domain.Core.Models;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    protected readonly DbSet<TEntity> _dbSet;
    protected readonly ApplicationDbContext _applicationDbContext;

    protected Repository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _dbSet = _applicationDbContext.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity obj)
    {
        await _dbSet.AddAsync(obj);
    }

    public virtual async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }
    
    public virtual void Update(TEntity obj)
    {
        _dbSet.Update(obj);
    }

    public virtual void Remove(TEntity obj)
    {
        _dbSet.Remove(obj);
    }
    
    public void Dispose()
    {
        _applicationDbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}