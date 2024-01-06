using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Bracket.RemoveAllBrackets;

public class RemoveAllBracketsHandler : ICommandHandler<RemoveAllBracketsCommand>
{
    private readonly IBracketRepository _bracketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveAllBracketsHandler(IBracketRepository bracketRepository, IUnitOfWork unitOfWork)
    {
        _bracketRepository = bracketRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveAllBracketsCommand request, CancellationToken cancellationToken)
    {
        var brackets = _bracketRepository.GetAll(request.Id);
        _bracketRepository.RemoveAll(brackets);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}