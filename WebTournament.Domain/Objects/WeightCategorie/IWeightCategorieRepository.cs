using WebTournament.Domain.Enums;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.WeightCategorie;

public interface IWeightCategorieRepository : IRepository<WeightCategorie>
{
    Task<bool> IsUnique(int maxWeight, string gender, Guid ageGroupId);
}