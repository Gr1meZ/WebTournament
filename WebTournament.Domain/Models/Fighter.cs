using System.ComponentModel.DataAnnotations.Schema;
using WebTournament.Domain.Core.Models;
using WebTournament.Domain.Enums;

namespace WebTournament.Domain.Models
{
    public class Fighter : Entity
    {
        public Fighter(Guid id, Guid tournamentId, Guid weightCategorieId, Guid beltId, Guid trainerId, Guid? bracketId, string name,
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

        public  Tournament Tournament { get; protected set;}
        public  WeightCategorie WeightCategorie { get; protected set;}
        public  Belt Belt { get; protected set;}  
        public  Trainer Trainer { get; protected set;}    
        public Bracket Bracket { get; protected set; }

    }
}
