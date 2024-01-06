using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.WeightCategorie.GetWeightCategorieList;

public sealed class WeightCategorieSpecification : BaseSpecification<Domain.Objects.WeightCategorie.WeightCategorie>
{
    public WeightCategorieSpecification(IQueryable<Domain.Objects.WeightCategorie.WeightCategorie> entities, string search, string orderColumn, string orderDir) 
        : base(entities, search, orderColumn, orderDir)
    {
        SearchData();
        OrderData();
    }

    protected override void SearchData()
    {
        if (!string.IsNullOrWhiteSpace(Search))
        {
            Entities = (Search.Split(' ')).Aggregate(Entities, (current, searchWord) =>
                current.Where(f =>
                    f.AgeGroup.Name.ToLower().Contains(searchWord.ToLower()) ||
                    f.MaxWeight.ToString().ToLower().Contains(searchWord.ToLower()) ||
                    f.WeightName.ToLower().Contains(searchWord.ToLower())
                ));
        }
    }

    protected override void OrderData()
    {
        if (!string.IsNullOrWhiteSpace(OrderColumn) && !string.IsNullOrWhiteSpace(OrderDir))
        {
            Entities = OrderColumn switch
            {
                "weightName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.WeightName)
                    : Entities.OrderByDescending(o => o.WeightName),
                "maxWeight" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.MaxWeight)
                    : Entities.OrderByDescending(o => o.MaxWeight),
                "ageGroupName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.AgeGroup.Name)
                    : Entities.OrderByDescending(o => o.AgeGroup.Name),
                "gender" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Gender)
                    : Entities.OrderByDescending(o => o.Gender),
                _ => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Id)
                    : Entities.OrderByDescending(o => o.Id)
            };
        }
    }
}