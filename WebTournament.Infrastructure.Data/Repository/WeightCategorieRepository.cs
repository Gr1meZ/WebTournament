using Microsoft.EntityFrameworkCore;
using CustomExceptionsLibrary;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class WeightCategorieRepository : Repository<WeightCategorie>, IWeightCategorieRepository
{
    public WeightCategorieRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task<bool> IsUnique(int maxWeight, string gender, Guid ageGroupId) =>
        await _applicationDbContext.WeightCategories.AnyAsync(x =>
            x.MaxWeight == maxWeight && x.AgeGroupId == ageGroupId && x.Gender == GenderExtension.ParseEnum(gender));
    
    public override IQueryable<WeightCategorie> GetAll()
    {
        return _dbSet.Include(x => x.AgeGroup).AsQueryable().AsNoTracking();
    }
    public override async Task<WeightCategorie> GetByIdAsync(Guid id)
    {
        var domain = await _dbSet
            .Include(x => x.AgeGroup)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (domain is null)
            throw new ValidationException("ValidationException", $"{nameof(WeightCategorie)} is not found");
        
        return domain;
    }
}