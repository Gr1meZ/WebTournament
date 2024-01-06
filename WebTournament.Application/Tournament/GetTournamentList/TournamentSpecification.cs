using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Tournament.GetTournamentList;

public sealed class TournamentSpecification : BaseSpecification<Domain.Objects.Tournament.Tournament>
{
    public TournamentSpecification(IQueryable<Domain.Objects.Tournament.Tournament> entities, string search, string orderColumn, string orderDir) : base(entities, search, orderColumn, orderDir)
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
                    f.StartDate.ToString().ToLower().Contains(searchWord.ToLower()) ||
                    f.Address.ToLower().Contains(searchWord.ToLower())
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
                "startDate" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.StartDate)
                    : Entities.OrderByDescending(o => o.StartDate),
                "address" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Address)
                    : Entities.OrderByDescending(o => o.Address),
                _ => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Id)
                    : Entities.OrderByDescending(o => o.Id)
            };
        }
    }
}