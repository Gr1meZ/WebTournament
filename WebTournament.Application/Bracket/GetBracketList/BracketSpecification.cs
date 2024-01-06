using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Bracket.GetBracketList;

public sealed class BracketSpecification : BaseSpecification<Domain.Objects.Bracket.Bracket>
{
    public BracketSpecification(IQueryable<Domain.Objects.Bracket.Bracket> entities, string search, string orderColumn, string orderDir) : base(entities, search, orderColumn, orderDir)
    {
        SearchData();
        OrderData();
    }

    protected override void SearchData()
    {
        if (!string.IsNullOrWhiteSpace(Search))
        {
            Entities = Search.Split(' ').Aggregate(Entities, (current, searchWord) =>
                current.Where(f =>
                    f.Tournament.Name.ToLower().Contains(searchWord.ToLower()) ||
                    f.WeightCategorie.WeightName.ToLower().Contains(searchWord.ToLower()) ||
                    f.WeightCategorie.MaxWeight.ToString().Contains(searchWord.ToLower()) ||
                    f.WeightCategorie.AgeGroup.Name.ToLower().Contains(searchWord.ToLower())
                ));
        }
    }

    protected override void OrderData()
    {
        if (!string.IsNullOrWhiteSpace(OrderColumn) && !string.IsNullOrWhiteSpace(OrderDir))
        {
            Entities = OrderColumn switch
            {
                "divisionName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Division)
                    : Entities.OrderByDescending(o => o.Division),
                "categoriesName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.WeightCategorie.AgeGroup.MinAge)
                    : Entities.OrderByDescending(o => o.WeightCategorie.AgeGroup.MinAge),
                "maxWeight" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.WeightCategorie.MaxWeight)
                    : Entities.OrderByDescending(o => o.WeightCategorie.MaxWeight),
                _ => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.WeightCategorie.MaxWeight)
                    : Entities.OrderByDescending(o => o.WeightCategorie.MaxWeight)
            };
        }
    }
}