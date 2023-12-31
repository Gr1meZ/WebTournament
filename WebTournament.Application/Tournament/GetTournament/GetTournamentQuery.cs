using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Tournament.GetTournament;

public class GetTournamentQuery : IQuery<TournamentResponse>
{
    public Guid Id { get; private set; }

    public GetTournamentQuery(Guid id) => Id = id;

}