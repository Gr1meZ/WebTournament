using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Tournament.GetTournamentResults;

public class GetTournamentResultsQuery : IQuery<List<BracketWinnerResponse>>
{
    public Guid Id { get; private set; }
    
    public GetTournamentResultsQuery(Guid id) => Id = id;
}