using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.Fighter.GetFighter;

public class GetFighterQuery : IQuery<FighterResponse>
{
    public Guid Id { get; private set; }
    public GetFighterQuery(Guid id) => Id = id;
}