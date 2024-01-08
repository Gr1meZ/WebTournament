using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class BeltRepository : Repository<Belt>, IBeltRepository
{
    public BeltRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task<bool> IsExistsAsync(int beltNumber, string shortName) => await _applicationDbContext.Belts
        .Where(x => x.BeltNumber == beltNumber && x.ShortName == shortName)
        .AnyAsync();

    public IQueryable<string> GetMatchedBeltsByDivision(Guid[] divison)
    {
        return _applicationDbContext.Belts.OrderBy(belt => belt.BeltNumber)
            .Where(belt => divison.Contains(belt.Id)).Select(y => $"{y.BeltNumber} {y.ShortName}");
    }
}