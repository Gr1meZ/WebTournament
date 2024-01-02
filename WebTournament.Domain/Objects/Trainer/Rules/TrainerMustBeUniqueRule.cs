using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Trainer.Rules;

public class TrainerMustBeUniqueRule : IBusinessRule
{
    private readonly string _name;
    private readonly string _surname;
    private readonly string _patronymic;
    private readonly string _phone;
    private readonly Guid _clubId;
    private readonly ITrainerRepository _trainerRepository;
    public TrainerMustBeUniqueRule(string name, string surname, string patronymic, string phone, Guid clubId, ITrainerRepository trainerRepository)
    {
        _name = name;
        _surname = surname;
        _patronymic = patronymic;
        _phone = phone;
        _clubId = clubId;
        _trainerRepository = trainerRepository;
    }

    public async Task<bool> IsBrokenAsync() =>
        await _trainerRepository.IsUnique(_name, _surname, _patronymic, _phone, _clubId);
   

    public string Message => "Данный тренер уже существует!";
}