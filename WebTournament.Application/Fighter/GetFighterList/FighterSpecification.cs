using WebTournament.Application.SeedPaging;

namespace WebTournament.Application.Fighter.GetFighterList;

public sealed class FighterSpecification : BaseSpecification<Domain.Objects.Fighter.Fighter>
{
    public FighterSpecification(IQueryable<Domain.Objects.Fighter.Fighter> entities, string search, string orderColumn, string orderDir) : base(entities, search, orderColumn, orderDir)
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
                    f.Age.ToString().ToLower().Contains(searchWord.ToLower()) ||
                    f.BirthDate.ToString().ToLower().Contains(searchWord.ToLower()) ||
                    f.City.ToLower().Contains(searchWord.ToLower()) ||
                    f.Surname.ToLower().Contains(searchWord.ToLower()) ||
                    f.Country.ToLower().Contains(searchWord.ToLower()) ||
                    f.Belt.ShortName.ToLower().Contains(searchWord.ToLower()) ||
                    f.Belt.BeltNumber.ToString().ToLower().Contains(searchWord.ToLower()) ||
                    f.Trainer.Surname.ToLower().Contains(searchWord.ToLower()) ||
                    f.WeightCategorie.WeightName.ToLower().Contains(searchWord.ToLower()) ||
                    f.WeightCategorie.AgeGroup.Name.ToLower().Contains(searchWord.ToLower()) ||
                    f.Trainer.Club.Name.ToLower().Contains(searchWord.ToLower())
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
                "age" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Age)
                    : Entities.OrderByDescending(o => o.Age),
                "birthDate" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.BirthDate)
                    : Entities.OrderByDescending(o => o.BirthDate),
                "city" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.City)
                    : Entities.OrderByDescending(o => o.City),
                "surname" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Surname)
                    : Entities.OrderByDescending(o => o.Surname),
                "country" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Country)
                    : Entities.OrderByDescending(o => o.Country),
                "gender" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Gender)
                    : Entities.OrderByDescending(o => o.Gender),
                "beltShortName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Belt.ShortName)
                    : Entities.OrderByDescending(o => o.Belt.ShortName),
                "trainerName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Trainer.Surname)
                    : Entities.OrderByDescending(o => o.Trainer.Surname),
                "weightCategorieName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.WeightCategorie.WeightName)
                    : Entities.OrderByDescending(o => o.WeightCategorie.WeightName),
                "clubName" => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Trainer.Club.Name)
                    : Entities.OrderByDescending(o => o.Trainer.Club.Name),
                _ => (OrderDir.Equals("asc"))
                    ? Entities.OrderBy(o => o.Id)
                    : Entities.OrderByDescending(o => o.Id)
            };
        }
    }
}