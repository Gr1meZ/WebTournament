using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Belt;

public interface IBeltRepository : IRepository<Belt>
{
    Task<bool> IsUniqueAsync(int beltNumber, string shortName);
    IQueryable<string> GetMatchedBeltsByDivision(Guid[] divison);

}