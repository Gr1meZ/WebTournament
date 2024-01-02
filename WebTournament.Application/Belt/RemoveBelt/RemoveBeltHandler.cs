using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Belt.RemoveBelt;

public class RemoveBeltHandler : ICommandHandler<RemoveBeltCommand>
{
    private readonly IBeltRepository _beltRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveBeltHandler(IUnitOfWork unitOfWork, IBeltRepository beltRepository)
    {
        _unitOfWork = unitOfWork;
        _beltRepository = beltRepository;
    }

    public async Task Handle(RemoveBeltCommand request, CancellationToken cancellationToken)
    {
        var belt = await _beltRepository.GetByIdAsync(request.Id);
        _beltRepository.Remove(belt);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}