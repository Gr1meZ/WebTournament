using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.WeightCategorie.CreateWeightCategorie;

public class CreateWeightCategorieHandler : ICommandHandler<CreateWeightCategorieCommand>
{
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateWeightCategorieHandler(IWeightCategorieRepository weightCategorieRepository, IUnitOfWork unitOfWork)
    {
        _weightCategorieRepository = weightCategorieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CreateWeightCategorieCommand request, CancellationToken cancellationToken)
    {
        var weightCategorie = await Domain.Objects.WeightCategorie.WeightCategorie
            .CreateAsync(request.AgeGroupId.Value, request.MaxWeight.Value, request.WeightName,
                GenderExtension.ParseEnum(request.Gender), _weightCategorieRepository);
       
        await _weightCategorieRepository.AddAsync(weightCategorie);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}