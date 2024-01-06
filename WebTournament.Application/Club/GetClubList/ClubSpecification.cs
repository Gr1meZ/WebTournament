using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Club.GetClubList;

public sealed class ClubSpecification : BaseSpecification<Domain.Objects.Club.Club>
{
    public ClubSpecification(IQueryable<Domain.Objects.Club.Club> entities, string search, string orderColumn, string orderDir) : base(entities, search, orderColumn, orderDir)
    {
        SearchData();
        OrderData();
    }

    protected override void SearchData()
    {
        if (!string.IsNullOrWhiteSpace(Search))
        {
            Entities = (Search.Split(' ')).Aggregate(Entities, (current, searchWord) =>
                current.Where(f => f.Name.ToLower().Contains(searchWord.ToLower())));
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
                _ => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Id)
                    : Entities.OrderByDescending(o => o.Id)
            };
        }
    }
}