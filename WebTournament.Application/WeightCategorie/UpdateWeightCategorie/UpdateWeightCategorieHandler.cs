using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.WeightCategorie.UpdateWeightCategorie;

public class UpdateWeightCategorieHandler : ICommandHandler<UpdateWeightCategorieCommand>
{
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWeightCategorieHandler(IWeightCategorieRepository weightCategorieRepository, IUnitOfWork unitOfWork)
    {
        _weightCategorieRepository = weightCategorieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateWeightCategorieCommand request, CancellationToken cancellationToken)
    {
        var weightCategorie = await _weightCategorieRepository.GetByIdAsync(request.Id);
        weightCategorie.Change(request.AgeGroupId.Value, request.MaxWeight.Value, request.WeightName, GenderExtension.ParseEnum(request.Gender));

        await _unitOfWork.CommitAsync(cancellationToken);
    }
}