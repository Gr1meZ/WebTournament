using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Belt.UpdateBelt;

public class UpdateBeltHandler : ICommandHandler<UpdateBeltCommand>
{
    private readonly IBeltRepository _beltRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBeltHandler(IBeltRepository beltRepository, IUnitOfWork unitOfWork)
    {
        _beltRepository = beltRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateBeltCommand request, CancellationToken cancellationToken)
    {
        var belt = await _beltRepository.GetByIdAsync(request.Id);
        belt.Change(request.BeltNumber!.Value, request.ShortName, request.FullName);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}