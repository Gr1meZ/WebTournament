using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Fighter.GetFighter;

public class GetFighterQuery : IQuery<FighterDto>
{
    public Guid Id { get; private set; }
    public GetFighterQuery(Guid id) => Id = id;
}