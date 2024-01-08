using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Club;

public interface IClubRepository : IRepository<Club>
{
    Task<bool> IsExistsAsync(string name);

}