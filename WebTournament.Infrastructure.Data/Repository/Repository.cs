using Microsoft.EntityFrameworkCore;
using CustomExceptionsLibrary;
using WebTournament.Domain.SeedWork;
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
        var domain = await _dbSet.FindAsync(id);
        
        if (domain is null)
            throw new ValidationException("ValidationException", $"{nameof(TEntity)} is not found");
        
        return domain;
    }

    public virtual IQueryable<TEntity> GetAll()
    {
        return _dbSet.AsQueryable().AsNoTracking();
    }
    
    public virtual void Update(TEntity obj)
    {
        _dbSet.Update(obj);
    }

    public virtual void Remove(TEntity obj)
    {
        _dbSet.Remove(obj);
    }
    
    public virtual void RemoveAll(IQueryable<TEntity> objQuery)
    {
        _dbSet.RemoveRange(objQuery);
    }

    public void Dispose()
    {
        _applicationDbContext.Dispose();
        GC.SuppressFinalize(this);
    }
    
    
}