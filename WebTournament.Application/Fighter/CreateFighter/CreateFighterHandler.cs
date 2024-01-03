using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.Fighter;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Fighter.CreateFighter;

public class CreateFighterHandler : ICommandHandler<CreateFighterCommand>
{
    private readonly IFighterRepository _fighterRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateFighterHandler(IFighterRepository fighterRepository, IUnitOfWork unitOfWork)
    {
        _fighterRepository = fighterRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateFighterCommand request, CancellationToken cancellationToken)
    {
        var fighter = await Domain.Objects.Fighter.Fighter.CreateAsync(request.TournamentId.Value, request.WeightCategorieId.Value,
            request.BeltId.Value, request.TrainerId.Value, null, request.Name, request.Surname, request.BirthDate, request.Country,
            request.City, request.Gender, _fighterRepository);

        await _fighterRepository.AddAsync(fighter);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}