using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.AgeGroup;

public interface IAgeGroupRepository : IRepository<AgeGroup>
{
    Task<bool> IsUnique(int? minAge, int? maxAge);
}