using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.SeedWork;
using static WebTournament.Domain.Objects.AgeGroup.AgeGroup;

namespace WebTournament.Application.AgeGroup.CreateAgeGroup;

public class CreateAgeGroupHandler : ICommandHandler<CreateAgeGroupCommand>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateAgeGroupHandler(IAgeGroupRepository ageGroupRepository, IUnitOfWork unitOfWork)
    {
        _ageGroupRepository = ageGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateAgeGroupCommand request, CancellationToken cancellationToken)
    {
        var ageGroup = await CreateAsync(Guid.NewGuid(), request.Name, request.MinAge, request.MaxAge, _ageGroupRepository);
        await _ageGroupRepository.AddAsync(ageGroup);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}