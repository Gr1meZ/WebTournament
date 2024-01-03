using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Fighter;

public interface IFighterRepository : IRepository<Fighter>
{
    Task<bool> IsUnique(string surname, string name, string city, Guid tournamentId);
    IQueryable<Fighter> GetAll(Guid tournamentId);
    IQueryable<Fighter> GetAllByTournamentId(Guid tournamentId);
    Task AddRangeAsync(IEnumerable<Fighter> fighters);
}