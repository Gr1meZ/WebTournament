namespace Infrastructure.DataAccess.Abstract
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
    

    }
