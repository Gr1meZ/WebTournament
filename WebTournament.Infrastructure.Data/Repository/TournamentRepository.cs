using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.Objects.BracketWinner;
using WebTournament.Domain.Objects.Tournament;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class TournamentRepository : Repository<Tournament>, ITournamentRepository
{
    public TournamentRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task<bool> IsUnique(string name, string address) => 
        await _applicationDbContext.Tournaments.AnyAsync(x => x.Name == name && x.Address == address);

    public IQueryable<BracketWinner> GetTournamentResults(Guid tournamentId)
    {
        return  _applicationDbContext.BracketWinners.Where(x =>
                x.Bracket.TournamentId == tournamentId &&
                (x.FirstPlaceId != null || x.SecondPlaceId != null || x.ThirdPlaceId != null))
            .Include(x => x.Bracket.WeightCategorie.AgeGroup)
            .Include(x => x.FirstPlacePlayer.Trainer.Club)
            .Include(x => x.SecondPlacePlayer.Trainer.Club)
            .Include(x => x.ThirdPlacePlayer.Trainer.Club)
            .Include(x => x.FirstPlacePlayer.Tournament)
            .OrderBy(x => x.Bracket.WeightCategorie.AgeGroup.MinAge).ThenBy(x => x.Bracket.WeightCategorie.MaxWeight);
    }
}