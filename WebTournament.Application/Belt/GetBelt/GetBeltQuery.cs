using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.Belt.GetBelt;

public class GetBeltQuery : IQuery<BeltDto>
{
    public Guid Id { get; private set; }

    public GetBeltQuery(Guid id)
    {
        Id = id;
    }
}