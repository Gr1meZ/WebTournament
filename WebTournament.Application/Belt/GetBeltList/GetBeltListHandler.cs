using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;
using WebTournament.Domain.Objects.Belt;

namespace WebTournament.Application.Belt.GetBeltList;

public class GetBeltListHandler : IQueryHandler<GetBeltListQuery, PagedResponse<BeltResponse[]>>
{
    private readonly IBeltRepository _beltRepository;
    private readonly IMapper _mapper;

    public GetBeltListHandler(IBeltRepository beltRepository, IMapper mapper)
    {
        _beltRepository = beltRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<BeltResponse[]>> Handle(GetBeltListQuery request,
        CancellationToken cancellationToken)
    {
        var beltQuery = _beltRepository.GetAll();
        var beltSpecificationResult = await new BeltSpecification(beltQuery, request.Search, request.OrderColumn, request.OrderDir)
            .GetSpecificationResultAsync(request.PageNumber, request.PageSize, cancellationToken);

        var dbItems = await beltSpecificationResult.Entities
            .Select(x => _mapper.Map<BeltResponse>(x))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedResponse<BeltResponse[]>(dbItems, beltSpecificationResult.Total, request.PageNumber, request.PageSize);
    }
}