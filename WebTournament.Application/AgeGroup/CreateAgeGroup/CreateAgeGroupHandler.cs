using WebTournament.Application.Configuration;
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
        var isUnique = await _ageGroupRepository.IsUnique(request.MinAge, request.MaxAge);
        var ageGroup = Create(Guid.NewGuid(), request.Name, request.MinAge, request.MaxAge, isUnique);
        await _ageGroupRepository.AddAsync(ageGroup);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}