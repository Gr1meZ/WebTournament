using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.AgeGroup.GetAgeGroup;

public class GetAgeGroupQuery : IQuery<AgeGroupDto>
{
    public Guid Id { get; private set; }

    public GetAgeGroupQuery(Guid id)
    {
        Id = id;
    }
}