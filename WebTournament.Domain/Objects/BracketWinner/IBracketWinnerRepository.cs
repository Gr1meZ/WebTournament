using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.BracketWinner;

public interface IBracketWinnerRepository : IRepository<BracketWinner>
{
    Task AddRangeAsync(IEnumerable<BracketWinner> bracketWinners);
}