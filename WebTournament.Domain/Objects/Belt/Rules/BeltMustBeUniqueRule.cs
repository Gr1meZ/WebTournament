using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Belt.Rules;

public class BeltMustBeUniqueRule : IBusinessRule
{
    private readonly IBeltRepository _beltRepository;
    private readonly int _beltNumber;
    private readonly string _shortName;

    public BeltMustBeUniqueRule(IBeltRepository beltRepository, int beltNumber, string shortName)
    {
        _beltRepository = beltRepository;
        _beltNumber = beltNumber;
        _shortName = shortName;
    }

    public Task<bool> IsBrokenAsync() => _beltRepository.IsUniqueAsync(_beltNumber, _shortName);


    public string Message => "Данный пояс уже существует!";
}