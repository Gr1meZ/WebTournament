using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Bracket.RemoveBracket;

public class RemoveBracketHandler : ICommandHandler<RemoveBracketCommand>
{
    private readonly IBracketRepository _bracketRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveBracketHandler(IBracketRepository bracketRepository, IUnitOfWork unitOfWork)
    {
        _bracketRepository = bracketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveBracketCommand request, CancellationToken cancellationToken)
    {
        var bracket = await _bracketRepository.GetByIdAsync(request.Id);
        _bracketRepository.Remove(bracket);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}