using WebTournament.Application.Configuration.Queries;


namespace WebTournament.Application.Belt.GetBelt;

public class GetBeltQuery : IQuery<BeltResponse>
{
    public Guid Id { get; private set; }

    public GetBeltQuery(Guid id)
    {
        Id = id;
    }
}