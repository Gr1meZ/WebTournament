using Microsoft.EntityFrameworkCore;
using CustomExceptionsLibrary;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class FighterRepository : Repository<Fighter>, IFighterRepository
{
    public FighterRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task<bool> IsExistsAsync(string surname, string name, string city, Guid tournamentId) =>
        await _applicationDbContext.Fighters.AnyAsync(x =>
            x.Surname == surname && x.Name == name && x.City == city && x.TournamentId == tournamentId);

    public override async Task<Fighter> GetByIdAsync(Guid id)
    {
        var fighter = await _applicationDbContext.Fighters
            .Include(x => x.Tournament)
            .Include(x => x.Belt)
            .Include(x => x.Trainer)
            .Include(x => x.WeightCategorie.AgeGroup)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (fighter is null)
            throw new ValidationException("ValidationException", $"{nameof(fighter)} is not found");
        
        return fighter;
    }

    public IQueryable<Fighter> GetAll(Guid tournamentId)
    {
        return _applicationDbContext.Fighters
            .Include(x => x.Trainer.Club)
            .Include(x => x.Belt)
            .Include(x => x.Tournament)
            .Include(x => x.WeightCategorie.AgeGroup)
            .Where(x => x.TournamentId ==  tournamentId)
            .AsQueryable()
            .AsNoTracking();
    }

    public IQueryable<Fighter> GetAllByTournamentId(Guid tournamentId)
    {
        return _applicationDbContext.Fighters.Where(x => x.TournamentId == tournamentId);
    }

    public async Task AddRangeAsync(IEnumerable<Fighter> fighters)
    {
        await _applicationDbContext.Fighters.AddRangeAsync(fighters);
    }
}