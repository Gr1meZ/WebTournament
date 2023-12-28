using WebTournament.Application.Configuration;
using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.AgeGroup;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.AgeGroup.RemoveAgeGroup;

public class RemoveAgeGroupHandler : ICommandHandler<RemoveAgeGroupCommand>
{
    private readonly IAgeGroupRepository _ageGroupRepository;
    private readonly IUnitOfWork _unitOfWork;


    public RemoveAgeGroupHandler(IAgeGroupRepository ageGroupRepository, IUnitOfWork unitOfWork)
    {
        _ageGroupRepository = ageGroupRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveAgeGroupCommand request, CancellationToken cancellationToken)
    {
        var ageGroup = await _ageGroupRepository.GetByIdAsync(request.Id);
        _ageGroupRepository.Remove(ageGroup);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}