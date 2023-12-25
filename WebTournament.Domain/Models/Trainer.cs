using WebTournament.Domain.Core.Models;

namespace WebTournament.Domain.Models
{
    public class Trainer : Entity
    {
        public Trainer(Guid id, Guid clubId, Club club, string name, string surname, string patronymic, string phone)
        {
            Id = id;
            ClubId = clubId;
            Club = club;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Phone = phone;
        }
        protected Trainer() {}
        
        public Guid ClubId { get; private set; }
        public  Club Club { get; private set; }
        public  string Name { get; private set; }
        public  string Surname { get; private set; }
        public  string Patronymic { get; private set; }
        public  string Phone { get; private set; }
        
        public IReadOnlyCollection<Fighter> Fighters { get; protected set; }

    }
}
