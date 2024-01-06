using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.Objects.Tournament;

namespace WebTournament.Application.Tournament.GetTournamentResults;

public class GetTournamentResultsHandler : IQueryHandler<GetTournamentResultsQuery, List<BracketWinnerResponse>>
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IBeltRepository _beltRepository;
    public GetTournamentResultsHandler(ITournamentRepository tournamentRepository, IBeltRepository beltRepository)
    {
        _tournamentRepository = tournamentRepository;
        _beltRepository = beltRepository;
    }

    public async Task<List<BracketWinnerResponse>> Handle(GetTournamentResultsQuery request, CancellationToken cancellationToken)
    {
        var bracketResults = await _tournamentRepository
            .GetTournamentResults(request.Id)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return bracketResults.Select(x => new BracketWinnerResponse()
        {
            Id = x.Id,
            Division = string.Join(", ",  _beltRepository.GetMatchedBeltsByDivision(x.Bracket.Division)),
            TournamentName = x.FirstPlacePlayer.Tournament.Name,
            FirstPlayerClubName = x.FirstPlacePlayer?.Trainer?.Club?.Name ?? string.Empty,
            FirstPlayerFullName = $"{x.FirstPlacePlayer?.Surname} {x.FirstPlacePlayer?.Name}",
            SecondPlayerClubName = x.SecondPlacePlayer?.Trainer?.Club?.Name ?? string.Empty,
            SecondPlayerFullName = $"{x.SecondPlacePlayer?.Surname} {x.SecondPlacePlayer?.Name}",
            ThirdPlayerClubName = x.ThirdPlacePlayer?.Trainer?.Club?.Name ?? string.Empty,
            ThirdPlayerFullName = $"{x.ThirdPlacePlayer?.Surname} {x.ThirdPlacePlayer?.Name}",
            CategorieName = $"{x.FirstPlacePlayer?.WeightCategorie?.AgeGroup?.Name} - {x.FirstPlacePlayer?.WeightCategorie?.WeightName}"
        }).ToList();
    }
}