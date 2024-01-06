using WebTournament.Application.Configuration.Queries;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.Objects.Bracket;

namespace WebTournament.Application.Bracket.GetBracket;

public class GetBracketHandler : IQueryHandler<GetBracketQuery, BracketStateResponse>
{
    private readonly IBracketRepository _bracketRepository;
    private readonly IBeltRepository _beltRepository;

    public GetBracketHandler(IBracketRepository bracketRepository, IBeltRepository beltRepository)
    {
        _bracketRepository = bracketRepository;
        _beltRepository = beltRepository;
    }

    public async Task<BracketStateResponse> Handle(GetBracketQuery request, CancellationToken cancellationToken)
    {
        var bracket = await _bracketRepository.GetByIdAsync(request.Id);
        var bracketViewModel = new BracketStateResponse()
        {
            Id = bracket.Id,
            State = bracket.State,
            Winners = new List<Guid>(),
            CategorieName = $"{bracket.WeightCategorie.AgeGroup.Name} - {bracket.WeightCategorie.WeightName} - {string.Join(", ",  _beltRepository.GetAll().OrderBy(belt => belt.BeltNumber)
                .Where(belt => bracket.Division.Contains(belt.Id)).Select(y => $"{y.BeltNumber} {y.ShortName}"))}"
        };
        return bracketViewModel;
    }
}