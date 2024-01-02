using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class BeltRepository : Repository<Belt>, IBeltRepository
{
    public BeltRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task<bool> IsUniqueAsync(int beltNumber, string shortName) => await _applicationDbContext.Belts
        .Where(x => x.BeltNumber == beltNumber && x.ShortName == shortName)
        .AnyAsync();
    
}