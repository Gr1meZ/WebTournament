using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Tournament.GetTournamentResults;

public class GetTournamentResultsQuery : IQuery<List<BracketWinnerDto>>
{
    public Guid Id { get; private set; }
    
    public GetTournamentResultsQuery(Guid id) => Id = id;
}