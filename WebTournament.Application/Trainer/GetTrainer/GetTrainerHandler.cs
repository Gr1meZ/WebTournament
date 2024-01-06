using AutoMapper;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Domain.Objects.Trainer;

namespace WebTournament.Application.Trainer.GetTrainer;

public class GetTrainerHandler : IQueryHandler<GetTrainerQuery, TrainerResponse>
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IMapper _mapper;

    public GetTrainerHandler(ITrainerRepository trainerRepository, IMapper mapper)
    {
        _trainerRepository = trainerRepository;
        _mapper = mapper;
    }

    public async Task<TrainerResponse> Handle(GetTrainerQuery request, CancellationToken cancellationToken)
    {
        var trainer = await _trainerRepository.GetByIdAsync(request.Id);
        return _mapper.Map<TrainerResponse>(trainer);
    }
}