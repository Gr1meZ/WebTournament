using Microsoft.EntityFrameworkCore;
using CustomExceptionsLibrary;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class BracketRepository : Repository<Bracket>, IBracketRepository
{
    public BracketRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task<bool> IsUnique(Guid tournamentId, Guid weightCategorieId, Guid[] division) =>
     await _applicationDbContext.Brackets.AnyAsync(x => x.TournamentId == tournamentId 
                                                        && x.WeightCategorieId == weightCategorieId 
                                                        && x.Division == division);

    public async Task AddRangeAsync(IEnumerable<Bracket> brackets)
    {
        await _dbSet.AddRangeAsync(brackets);
    }

    public override async Task<Bracket> GetByIdAsync(Guid id)
    {
        var bracket = await _applicationDbContext.Brackets
            .Include(x => x.WeightCategorie.AgeGroup)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (bracket is null)
            throw new ValidationException("ValidationException", "Bracket not found");
        
        return bracket;
    }
    
    public IQueryable<Bracket> GetAll(Guid tournamentId)
    {
        return _dbSet
            .Where(x => x.TournamentId == tournamentId)
            .AsNoTracking();
    }
    
    public IQueryable<Bracket> GetByTournamentId(Guid tournamentId) => _dbSet.Where(x => x.TournamentId == tournamentId);
    
}