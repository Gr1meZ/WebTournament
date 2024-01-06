using WebTournament.Application.Configuration.Queries;

namespace WebTournament.Application.WeightCategorie.GetWeightCategorie;

public class GetWeightCategorieQuery : IQuery<WeightCategorieResponse>
{
    public Guid Id { get; private set; }
    public GetWeightCategorieQuery(Guid id) => Id = id;
}