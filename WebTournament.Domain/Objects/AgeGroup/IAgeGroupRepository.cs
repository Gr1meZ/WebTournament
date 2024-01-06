using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.AgeGroup;

public interface IAgeGroupRepository : IRepository<AgeGroup>
{
    Task<bool> IsExistsAsync(int? minAge, int? maxAge);
}