using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class AgeGroupRepository : Repository<AgeGroup>, IAgeGroupRepository
{
    public AgeGroupRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }
    
    public async Task<bool> IsUniqueAsync(int? minAge, int? maxAge) => 
        await _applicationDbContext.AgeGroups.AnyAsync(x => x.MaxAge == maxAge && x.MinAge == minAge);

   
}