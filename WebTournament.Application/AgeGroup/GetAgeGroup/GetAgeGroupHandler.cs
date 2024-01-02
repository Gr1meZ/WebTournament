using AutoMapper;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.AgeGroup;

namespace WebTournament.Application.AgeGroup.GetAgeGroup;

public class GetAgeGroupHandler : IQueryHandler<GetAgeGroupQuery, AgeGroupDto>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IMapper _mapper;
    public GetAgeGroupHandler(IAgeGroupRepository ageGroupRepository, IMapper mapper)
    {
        _ageGroupRepository = ageGroupRepository;
        _mapper = mapper;
    }

    public async Task<AgeGroupDto> Handle(GetAgeGroupQuery request, CancellationToken cancellationToken)
    {
        var ageGroup = await _ageGroupRepository.GetByIdAsync(request.Id);
        return _mapper.Map<AgeGroupDto>(ageGroup);
    }
}