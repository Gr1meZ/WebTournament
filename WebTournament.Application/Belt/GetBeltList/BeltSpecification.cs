using WebTournament.Application.AgeGroup.GetAgeGroupList;
using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Belt.GetBeltList;

public sealed class BeltSpecification : BaseSpecification<Domain.Objects.Belt.Belt>
{
    public BeltSpecification(IQueryable<Domain.Objects.Belt.Belt> entities, string search, string orderColumn, string orderDir) 
        : base(entities, search, orderColumn, orderDir)
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
                    f.ShortName.ToLower().Contains(searchWord.ToLower()) ||
                    f.FullName.ToLower().Contains(searchWord.ToLower()) ||
                    f.BeltNumber.ToString().ToLower().Contains(searchWord.ToLower())
                ));
        }

    }

    protected override void OrderData()
    {
        if (!string.IsNullOrWhiteSpace(OrderColumn) && !string.IsNullOrWhiteSpace(OrderDir))
        {
            Entities = OrderColumn switch
            {
                "shortName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.ShortName)
                    : Entities.OrderByDescending(o => o.ShortName),
                "fullName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.FullName)
                    : Entities.OrderByDescending(o => o.FullName),
                "beltNumber" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.BeltNumber)
                    : Entities.OrderByDescending(o => o.BeltNumber),
                _ => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Id)
                    : Entities.OrderByDescending(o => o.Id)
            };
        }
    }
}