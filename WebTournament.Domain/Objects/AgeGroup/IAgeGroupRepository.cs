using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.AgeGroup;

public interface IAgeGroupRepository : IRepository<AgeGroup>
{
    Task<bool> IsUniqueAsync(int? minAge, int? maxAge);
}