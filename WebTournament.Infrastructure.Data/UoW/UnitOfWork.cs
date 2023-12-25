using WebTournament.Domain.Abstract;
using WebTournament.Domain.Core.Models;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.UoW;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task Commit()
    {
        return _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}