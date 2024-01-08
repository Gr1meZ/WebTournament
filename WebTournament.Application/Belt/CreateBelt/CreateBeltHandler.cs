using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Belt.CreateBelt;

public class CreateBeltHandler : ICommandHandler<CreateBeltCommand>
{
    private readonly IBeltRepository _beltRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBeltHandler(IBeltRepository beltRepository, IUnitOfWork unitOfWork)
    {
        _beltRepository = beltRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateBeltCommand request, CancellationToken cancellationToken)
    {
        var belt = await Domain.Objects.Belt.Belt.CreateAsync(Guid.NewGuid(), request.BeltNumber!.Value, 
            request.ShortName, request.FullName, _beltRepository);
        
        await _beltRepository.AddAsync(belt);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}