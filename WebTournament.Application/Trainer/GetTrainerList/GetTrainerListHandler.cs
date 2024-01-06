using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;
using WebTournament.Domain.Objects.Trainer;

namespace WebTournament.Application.Trainer.GetTrainerList;

public class GetTrainerListHandler : IQueryHandler<GetTrainerListQuery, PagedResponse<TrainerResponse[]>>
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IMapper _mapper;

    public GetTrainerListHandler(ITrainerRepository trainerRepository, IMapper mapper)
    {
        _trainerRepository = trainerRepository;
        _mapper = mapper;
    }

    public async Task<PagedResponse<TrainerResponse[]>> Handle(GetTrainerListQuery request,
        CancellationToken cancellationToken)
    {
        var trainersQuery = _trainerRepository.GetAll();

        var trainerSpecificationResult = await 
            new TrainerSpecification(trainersQuery, request.Search, request.OrderColumn, request.OrderDir)
                .GetSpecificationResultAsync(request.PageNumber, request.PageSize, cancellationToken);
        
        var dbItems = await trainerSpecificationResult.Entities
            .Select(x => _mapper.Map<TrainerResponse>(x))
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedResponse<TrainerResponse[]>(dbItems, trainerSpecificationResult.Total, request.PageNumber, request.PageSize);
    }
}