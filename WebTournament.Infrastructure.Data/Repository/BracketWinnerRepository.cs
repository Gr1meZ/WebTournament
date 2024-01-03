using WebTournament.Domain.Objects.BracketWinner;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class BracketWinnerRepository : Repository<BracketWinner>, IBracketWinnerRepository
{
    public BracketWinnerRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task AddRangeAsync(IEnumerable<BracketWinner> bracketWinners)
    {
        await _dbSet.AddRangeAsync(bracketWinners);
    }
}