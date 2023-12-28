using WebTournament.Domain.SeedWork;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}