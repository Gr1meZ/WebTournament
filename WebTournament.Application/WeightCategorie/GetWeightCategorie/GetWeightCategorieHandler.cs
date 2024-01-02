using AutoMapper;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.WeightCategorie;

namespace WebTournament.Application.WeightCategorie.GetWeightCategorie;

public class GetWeightCategorieHandler : IQueryHandler<GetWeightCategorieQuery, WeightCategorieDto>
{
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IMapper _mapper;
    public GetWeightCategorieHandler(IWeightCategorieRepository weightCategorieRepository, IMapper mapper)
    {
        _weightCategorieRepository = weightCategorieRepository;
        _mapper = mapper;
    }

    public async Task<WeightCategorieDto> Handle(GetWeightCategorieQuery request, CancellationToken cancellationToken)
    {
       var weightCategorie = await _weightCategorieRepository.GetByIdAsync(request.Id);
       return _mapper.Map<WeightCategorieDto>(weightCategorie);
    }
}