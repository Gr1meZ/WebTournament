using Microsoft.EntityFrameworkCore;
using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.SeedPaging;
using WebTournament.Domain.Objects.Belt;
using WebTournament.Domain.Objects.Bracket;

namespace WebTournament.Application.Bracket.GetBracketList;

public class GetBracketListHandler : IQueryHandler<GetBracketListQuery, PagedResponse<BracketResponse[]>>
{
    private readonly IBracketRepository _bracketRepository;
    private readonly IBeltRepository _beltRepository;

    public GetBracketListHandler(IBracketRepository bracketRepository, IBeltRepository beltRepository)
    {
        _bracketRepository = bracketRepository;
        _beltRepository = beltRepository;
    }

    public async Task<PagedResponse<BracketResponse[]>> Handle(GetBracketListQuery request,
        CancellationToken cancellationToken)
    {
        var bracketQuery = _bracketRepository
            .GetAll(request.TournamentId)
            .Include(x => x.Tournament)
            .Include(x => x.WeightCategorie.AgeGroup)
            .AsNoTracking();

        var bracketSpecificationResult = await 
            new BracketSpecification(bracketQuery, request.Search, request.OrderColumn, request.OrderDir)
                .GetSpecificationResult(request.PageNumber, request.PageSize, cancellationToken);

        var dbItems = await bracketSpecificationResult.Entities.Select(x => new BracketResponse()
        {
            Id = x.Id,
            DivisionName = string.Join(", ", _beltRepository.GetAll().OrderBy(belt => belt.BeltNumber)
                .Where(belt => x.Division.Contains(belt.Id)).Select(y => $"{y.BeltNumber} {y.ShortName}")),
            CategoriesName = $"{x.WeightCategorie.AgeGroup.Name} - {x.WeightCategorie.WeightName}",
            MaxWeight = x.WeightCategorie.MaxWeight
        }).ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedResponse<BracketResponse[]>(dbItems, bracketSpecificationResult.Total, request.PageNumber, request.PageSize);
    }
}