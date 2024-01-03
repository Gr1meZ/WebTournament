using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Bracket.Validators;
using WebTournament.Application.Configuration.Commands;
using WebTournament.Domain.Exceptions;
using WebTournament.Domain.Objects.Bracket;
using WebTournament.Domain.Objects.BracketWinner;
using WebTournament.Domain.Objects.WeightCategorie;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Application.Bracket.GenerateBracket;

public class GenerateBracketHandler : ICommandHandler<GenerateBracketCommand>
{
    private readonly IBracketRepository _bracketRepository;
    private readonly IWeightCategorieRepository _weightCategorieRepository;
    private readonly IUnitOfWork _unitOfWork;
    public GenerateBracketHandler(IBracketRepository bracketRepository, IUnitOfWork unitOfWork, IWeightCategorieRepository weightCategorieRepository, IBracketWinnerRepository bracketWinnerRepository)
    {
        _bracketRepository = bracketRepository;
        _unitOfWork = unitOfWork;
        _weightCategorieRepository = weightCategorieRepository;
    }

    public async Task Handle(GenerateBracketCommand request, CancellationToken cancellationToken)
    {
        BracketValidator.ValidateAgeGroup(request.AgeGroupId);
        
        var weightCategoriesIds = await _weightCategorieRepository.GetAll()
            .Where(x => x.AgeGroupId == request!.AgeGroupId)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var bracketsList = await Task.WhenAll(weightCategoriesIds.Select(async x =>
            await Domain.Objects.Bracket.Bracket.CreateAsync(x, request.TournamentId, request.Division, string.Empty,
                _bracketRepository)));

        await _bracketRepository.AddRangeAsync(bracketsList);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
    
}