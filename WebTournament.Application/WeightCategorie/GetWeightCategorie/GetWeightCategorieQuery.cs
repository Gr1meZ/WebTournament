using WebTournament.Application.Configuration.Queries;
using WebTournament.Application.DTO;

namespace WebTournament.Application.WeightCategorie.GetWeightCategorie;

public class GetWeightCategorieQuery : IQuery<WeightCategorieDto>
{
    public Guid Id { get; private set; }
    public GetWeightCategorieQuery(Guid id) => Id = id;
}