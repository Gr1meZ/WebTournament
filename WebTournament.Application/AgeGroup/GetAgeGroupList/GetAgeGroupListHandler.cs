using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;
using WebTournament.Domain.Objects.AgeGroup;

namespace WebTournament.Application.AgeGroup.GetAgeGroupList;

public class GetAgeGroupListHandler : IQueryHandler<GetAgeGroupListQuery, PagedResponse<AgeGroupResponse[]>>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IMapper _mapper;
    public GetAgeGroupListHandler(IAgeGroupRepository ageGroupRepository, IMapper mapper)
    {
        _ageGroupRepository = ageGroupRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<AgeGroupResponse[]>> Handle(GetAgeGroupListQuery request, CancellationToken cancellationToken)
    {
         var ageGroupQuery = _ageGroupRepository.GetAll();
         
         var ageGroupSpecificationResult = await new AgeGroupSpecification(ageGroupQuery, request.Search, request.OrderColumn, request.OrderDir)
             .GetSpecificationResultAsync(request.PageNumber, request.PageSize, cancellationToken);
         
            var dbItems =  await ageGroupSpecificationResult.Entities
                .Select(x => _mapper.Map<AgeGroupResponse>(x))
                .ToArrayAsync(cancellationToken: cancellationToken);

            return new PagedResponse<AgeGroupResponse[]>(dbItems, ageGroupSpecificationResult.Total, request.PageNumber, request.PageSize);
    }
}