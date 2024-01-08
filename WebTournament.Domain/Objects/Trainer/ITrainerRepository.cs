using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Trainer;

public interface ITrainerRepository : IRepository<Trainer>
{
    Task<bool> IsExistsAsync(string name, string surname, string patronymic , string phone, Guid clubId);
}