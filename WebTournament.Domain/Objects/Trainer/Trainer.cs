using WebTournament.Domain.Objects.Trainer.Rules;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Trainer
{
    public class Trainer : Entity
    {
        private Trainer(Guid id, Guid clubId, string name, string surname, string patronymic, string phone)
        {
            Id = id;
            ClubId = clubId;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Phone = phone;
        }
        protected Trainer() {}
        
        public Guid ClubId { get; private set; }
        public  Club.Club Club { get; private set; }
        public  string Name { get; private set; }
        public  string Surname { get; private set; }
        public  string Patronymic { get; private set; }
        public  string Phone { get; private set; }
        
        public IReadOnlyCollection<Fighter.Fighter> Fighters { get; protected set; }

        public static async Task<Trainer> CreateAsync(Guid id, string name, string surname, string patronymic, string phone,
            Guid clubId, ITrainerRepository trainerRepository)
        {
            await CheckRuleAsync(new TrainerMustBeUniqueRule(name, surname, patronymic, phone, clubId, trainerRepository));
            return new Trainer(id, clubId, name, surname, patronymic, phone);
        }
        
        public void Change(string name, string surname, string patronymic, string phone,
            Guid clubId)
        {
            ClubId = clubId;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Phone = phone;
        }
    }
}
