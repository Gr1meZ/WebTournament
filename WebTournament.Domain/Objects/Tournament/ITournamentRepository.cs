using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Tournament;

public interface ITournamentRepository : IRepository<Tournament>
{
    Task<bool> IsUnique(string name, string address);
    IQueryable<BracketWinner.BracketWinner> GetTournamentResults(Guid tournamentId);
}