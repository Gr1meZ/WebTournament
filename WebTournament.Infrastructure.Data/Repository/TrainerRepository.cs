using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class TrainerRepository : Repository<Trainer>, ITrainerRepository
{
    public TrainerRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task<bool> IsUnique(string name, string surname, string patronymic, string phone, Guid clubId) =>
        await _applicationDbContext.Trainers
            .AnyAsync(x =>
                x.Name == name && x.Surname == surname && x.Patronymic == patronymic && x.Phone == phone &&
                x.ClubId == clubId);

    public override IQueryable<Trainer> GetAll()
    {
        return _dbSet.Include(x => x.Club).AsQueryable().AsNoTracking();
    }
}