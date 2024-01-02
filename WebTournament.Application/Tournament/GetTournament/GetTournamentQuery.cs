using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Tournament.GetTournament;

public class GetTournamentQuery : IQuery<TournamentDto>
{
    public Guid Id { get; private set; }

    public GetTournamentQuery(Guid id) => Id = id;

}