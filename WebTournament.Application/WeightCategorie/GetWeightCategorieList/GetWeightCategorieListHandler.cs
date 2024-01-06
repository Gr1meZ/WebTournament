using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;
using WebTournament.Domain.Objects.WeightCategorie;

namespace WebTournament.Application.WeightCategorie.GetWeightCategorieList;

public class
    GetWeightCategorieListHandler : IQueryHandler<GetWeightCategorieListQuery, PagedResponse<WeightCategorieResponse[]>>
{
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IMapper _mapper;

    public GetWeightCategorieListHandler(IWeightCategorieRepository weightCategorieRepository, IMapper mapper)
    {
        _weightCategorieRepository = weightCategorieRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<WeightCategorieResponse[]>> Handle(GetWeightCategorieListQuery request,
        CancellationToken cancellationToken)
    {
        var weightCategorieQuery = _weightCategorieRepository.GetAll();
        
        var weightCategorieSpecificationResult = await 
            new WeightCategorieSpecification(weightCategorieQuery, request.Search, request.OrderColumn, request.OrderDir)
                .GetSpecificationResultAsync(request.PageNumber, request.PageSize, cancellationToken);
        
        var dbItems = await weightCategorieSpecificationResult.Entities
            .Select(x => _mapper.Map<WeightCategorieResponse>(x))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedResponse<WeightCategorieResponse[]>(dbItems, weightCategorieSpecificationResult.Total, request.PageNumber, request.PageSize);
    }
}