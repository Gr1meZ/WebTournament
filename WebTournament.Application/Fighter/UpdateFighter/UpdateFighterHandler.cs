using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Fighter.UpdateFighter;

public class UpdateFighterHandler : ICommandHandler<UpdateFighterCommand>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateFighterHandler(IFighterRepository fighterRepository, IUnitOfWork unitOfWork)
    {
        _fighterRepository = fighterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateFighterCommand request, CancellationToken cancellationToken)
    {
        var fighter = await _fighterRepository.GetByIdAsync(request.Id);
        fighter.Change(request.Name, request.BirthDate, request.BeltId.Value, request.City, request.Country, request.Gender,
            request.Surname, request.TournamentId.Value, request.TrainerId.Value, request.WeightCategorieId.Value);
        
        await _unitOfWork.CommitAsync(cancellationToken);

    }
}