using WebTournament.Application.Configuration;
using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.AgeGroup.UpdateAgeGroup;

public class UpdateAgeGroupHandler : ICommandHandler<UpdateAgeGroupCommand>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAgeGroupHandler(IAgeGroupRepository ageGroupRepository, IUnitOfWork unitOfWork)
    {
        _ageGroupRepository = ageGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateAgeGroupCommand request, CancellationToken cancellationToken)
    {
        var ageGroup = await _ageGroupRepository.GetByIdAsync(request.Id);
        ageGroup.Change(request.Name, request.MinAge, request.MaxAge);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}


