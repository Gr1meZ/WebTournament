using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.AgeGroup.GetAgeGroup;

public class GetAgeGroupQuery : IQuery<AgeGroupResponse>
{
    public Guid Id { get; private set; }

    public GetAgeGroupQuery(Guid id)
    {
        Id = id;
    }
}