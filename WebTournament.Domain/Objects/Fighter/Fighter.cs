using System.ComponentModel.DataAnnotations.Schema;
using WebTournament.Domain.Enums;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.Fighter.Rules;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Fighter
{
    public class Fighter : Entity
    {
        private Fighter(Guid id, Guid tournamentId, Guid weightCategorieId, Guid beltId, Guid trainerId, Guid? bracketId, string name,
            string surname, DateTime birthDate, int age, string country, string city, Gender gender)
        {
            Id = id;
            TournamentId = tournamentId;
            WeightCategorieId = weightCategorieId;
            BeltId = beltId;
            TrainerId = trainerId;
            BracketId = bracketId;
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
            Age = age;
            Country = country;
            City = city;
            Gender = gender;
        }
        
        protected Fighter() {}
        
        public Guid TournamentId { get; private set; }
        public Guid WeightCategorieId { get; private set; }
        public Guid BeltId { get; private set; }
        public Guid TrainerId { get; private set; }
        public Guid? BracketId { get; private set; }
        public  string Name { get; private set; }
        public  string Surname { get; private set; }
        [Column(TypeName = "timestamp")]
        public DateTime BirthDate { get; private set; }
        public int Age { get; private set; }
        public  string Country { get; private set; }
        public  string City { get; private set; }
        public Gender Gender { get; private set; }

        public  Tournament.Tournament Tournament { get; protected set;}
        public  WeightCategorie.WeightCategorie WeightCategorie { get; protected set;}
        public  Belt.Belt Belt { get; protected set;}  
        public  Trainer.Trainer Trainer { get; protected set;}    
        public Bracket.Bracket Bracket { get; protected set; }

        public static async Task<Fighter> CreateAsync(Guid tournamentId, Guid weightCategorieId, Guid beltId,
            Guid trainerId, Guid? bracketId, string name,
            string surname, DateTime birthDate, string country, string city, string gender, IFighterRepository fighterRepository)
        {
            await CheckRuleAsync(new FighterMustBeUniqueRule(fighterRepository, surname, name, city, tournamentId));
            return new Fighter(Guid.NewGuid(), tournamentId, weightCategorieId, beltId, trainerId, bracketId, name,
                surname, birthDate, AgeCalculator.CalculateAge(birthDate), country, city, GenderExtension.ParseEnum(gender));
        }

        public void Change(string name, DateTime birthDate, Guid beltId, string city, string country, string gender, string surname, Guid tournamentId, Guid trainerId, Guid weightCategorieId)
        {
            Name = name;
            BirthDate = birthDate;
            Age = AgeCalculator.CalculateAge(birthDate);
            BeltId = beltId;
            City = city;
            Country = country;
            Gender = GenderExtension.ParseEnum(gender);
            Surname = surname;
            TournamentId = tournamentId;
            TrainerId = trainerId;
            WeightCategorieId = weightCategorieId;
        }

        public void SetBracket(Guid bracketId) => BracketId = bracketId;

    }
}
