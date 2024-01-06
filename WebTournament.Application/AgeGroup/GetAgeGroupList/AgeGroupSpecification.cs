using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.AgeGroup.GetAgeGroupList;

public sealed class AgeGroupSpecification : BaseSpecification<Domain.Objects.AgeGroup.AgeGroup>
{
    public AgeGroupSpecification(IQueryable<Domain.Objects.AgeGroup.AgeGroup> entities, string search, string orderColumn, string orderDir) 
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
                    f.Name.ToLower().Contains(searchWord.ToLower()) ||
                    f.MinAge.ToString().ToLower().Contains(searchWord.ToLower()) ||
                    f.MaxAge.ToString().ToLower().Contains(searchWord.ToLower())
                ));
        }
    }

    protected override void OrderData()
    {
        if (!string.IsNullOrWhiteSpace(OrderColumn) && !string.IsNullOrWhiteSpace(OrderDir))
        {
            Entities = OrderColumn switch
            {
                "name" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Name)
                    : Entities.OrderByDescending(o => o.Name),
                "minAge" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.MinAge)
                    : Entities.OrderByDescending(o => o.MinAge),
                "maxAge" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.MaxAge)
                    : Entities.OrderByDescending(o => o.MaxAge),
                _ => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Id)
                    : Entities.OrderByDescending(o => o.Id)
            };
        }
    }
}