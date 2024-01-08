using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Bracket;

public interface IBracketRepository : IRepository<Bracket>
{
    Task<bool> IsExistsAsync(Guid tournamentId, Guid weightCategorieId, Guid[] division);
    Task AddRangeAsync(IEnumerable<Bracket> brackets);
    IQueryable<Bracket> GetByTournamentId(Guid tournamentId);
    IQueryable<Bracket> GetAll(Guid tournamentId);
}