using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.WeightCategorie;

public interface IWeightCategorieRepository : IRepository<WeightCategorie>
{
    Task<bool> IsExistsAsync(int maxWeight, string gender, Guid ageGroupId);
}