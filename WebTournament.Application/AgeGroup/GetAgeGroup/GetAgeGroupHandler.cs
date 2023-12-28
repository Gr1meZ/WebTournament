using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.AgeGroup;

namespace WebTournament.Application.AgeGroup.GetAgeGroup;

public class GetAgeGroupHandler : IQueryHandler<GetAgeGroupQuery, AgeGroupDto>
{
    private readonly IAgeGroupRepository _ageGroupRepository;

    public GetAgeGroupHandler(IAgeGroupRepository ageGroupRepository)
    {
        _ageGroupRepository = ageGroupRepository;
    }

    public async Task<AgeGroupDto> Handle(GetAgeGroupQuery request, CancellationToken cancellationToken)
    {
        var ageGroup = await _ageGroupRepository.GetByIdAsync(request.Id);
        var ageGroupDto = new AgeGroupDto()
        {
            Id = ageGroup.Id,
            Name = ageGroup.Name,
            MaxAge = ageGroup.MaxAge,
            MinAge = ageGroup.MinAge
        };
        return ageGroupDto;
    }
}