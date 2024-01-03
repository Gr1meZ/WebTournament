using WebTournament.Application.Configuration.Commands;
using WebTournament.Application.DTO;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.Domain.Objects.BracketWinner;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Bracket.SaveBracketState;

public class SaveBracketStateHandler : ICommandHandler<SaveBracketStateCommand>
{
    private readonly IBracketRepository _bracketRepository;
    private readonly IBracketWinnerRepository _bracketWinnerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SaveBracketStateHandler(IBracketRepository bracketRepository, IUnitOfWork unitOfWork,
        IBracketWinnerRepository bracketWinnerRepository)
    {
        _bracketRepository = bracketRepository;
        _unitOfWork = unitOfWork;
        _bracketWinnerRepository = bracketWinnerRepository;
    }

    public async Task Handle(SaveBracketStateCommand request, CancellationToken cancellationToken)
    {
        var bracket = await _bracketRepository.GetByIdAsync(request.Id);
        bracket.UpdateState(request.BracketState.State);
        await SyncWinners(request.BracketState);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    private async Task SyncWinners(BracketState bracketState)
    {

        if (bracketState?.Winners?.Count == null)
            return;

        var bracketWinner = await _bracketWinnerRepository.GetByIdAsync(bracketState.Id);

        Guid? GetWinnerId(int index)
        {
            return bracketState.Winners.ElementAtOrDefault(index) == Guid.Empty ? null : bracketState.Winners[index];
        }

        if (bracketWinner is null)
        {
            bracketWinner = BracketWinner.Create(bracketState.Id, GetWinnerId(0), GetWinnerId(1), GetWinnerId(2));
            await _bracketWinnerRepository.AddAsync(bracketWinner);
        }
        else
        {
            bracketWinner.Change(GetWinnerId(0), GetWinnerId(1), GetWinnerId(2));
        }
    }
}