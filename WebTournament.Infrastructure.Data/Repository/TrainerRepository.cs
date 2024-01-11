using CustomExceptionsLibrary;
using Microsoft.EntityFrameworkCore;
using WebTournament.Domain.Objects.Trainer;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.Data.Repository;

public class TrainerRepository : Repository<Trainer>, ITrainerRepository
{
    public TrainerRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public async Task<bool> IsExistsAsync(string name, string surname, string patronymic, string phone, Guid clubId) =>
        await _applicationDbContext.Trainers
            .AnyAsync(x =>
                x.Name == name && x.Surname == surname && x.Patronymic == patronymic && x.Phone == phone &&
                x.ClubId == clubId);

    public override IQueryable<Trainer> GetAll()
    {
        return _dbSet.Include(x => x.Club).AsQueryable().AsNoTracking();
    }

    public override async Task<Trainer> GetByIdAsync(Guid id)
    {
        var trainer = await _dbSet
            .Include(x => x.Club)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if (trainer is null)
            throw new ValidationException("ValidationException", $"{nameof(trainer)} is not found");
        
        return trainer;
    }
}