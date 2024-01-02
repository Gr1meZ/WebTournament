using AutoMapper;
using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.WeightCategorie.RemoveWeightCategorie;

public class RemoveWeightCategorieHandler : ICommandHandler<RemoveWeightCategorieCommand>
{
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RemoveWeightCategorieHandler(IWeightCategorieRepository weightCategorieRepository, IUnitOfWork unitOfWork)
    {
        _weightCategorieRepository = weightCategorieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveWeightCategorieCommand request, CancellationToken cancellationToken)
    {
        var weightCategorie = await _weightCategorieRepository.GetByIdAsync(request.Id);
        
        _weightCategorieRepository.Remove(weightCategorie);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}