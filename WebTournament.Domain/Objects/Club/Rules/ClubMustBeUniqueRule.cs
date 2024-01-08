using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Club.Rules;

public class ClubMustBeUniqueRule : IBusinessRule
{
    private readonly IClubRepository _clubRepository;
    private readonly string _name;

    public ClubMustBeUniqueRule(IClubRepository clubRepository, string name)
    {
        _clubRepository = clubRepository;
        _name = name;
    }

    public async Task<bool> IsBrokenAsync() => await _clubRepository.IsExistsAsync(_name);


    public string Message => "Данный клуб уже существует!";
}