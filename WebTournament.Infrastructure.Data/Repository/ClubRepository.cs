using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.Objects.Club;
using WebTournament.Domain.SeedWork;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class ClubRepository : Repository<Club>, IClubRepository
{
    public ClubRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task<bool> IsUniqueAsync(string name) => await _applicationDbContext.Clubs.AnyAsync(x => x.Name == name);

}