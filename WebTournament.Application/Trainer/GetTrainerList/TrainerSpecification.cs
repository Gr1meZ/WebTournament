using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Trainer.GetTrainerList;

public sealed class TrainerSpecification : BaseSpecification<Domain.Objects.Trainer.Trainer>
{
    public TrainerSpecification(IQueryable<Domain.Objects.Trainer.Trainer> entities, string search, string orderColumn, string orderDir) : base(entities, search, orderColumn, orderDir)
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
                    f.Surname.ToLower().Contains(searchWord.ToLower()) ||
                    f.Patronymic.ToLower().Contains(searchWord.ToLower()) ||
                    f.Phone.ToLower().Contains(searchWord.ToLower()) ||
                    f.Club.Name.ToLower().Contains(searchWord.ToLower())
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
                "surname" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Surname)
                    : Entities.OrderByDescending(o => o.Surname),
                "patronymic" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Patronymic)
                    : Entities.OrderByDescending(o => o.Patronymic),
                "phone" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Phone)
                    : Entities.OrderByDescending(o => o.Phone),
                _ => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Id)
                    : Entities.OrderByDescending(o => o.Id)
            };
        }
    }
}