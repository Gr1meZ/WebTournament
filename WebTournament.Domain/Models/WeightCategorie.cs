using WebTournament.Domain.Core.Models;
using WebTournament.Domain.Enums;

namespace WebTournament.Domain.Models
{
    public class WeightCategorie : Entity
    {
        public WeightCategorie(Guid id, Guid ageGroupId, AgeGroup ageGroup, int maxWeight, string weightName, Gender gender)
        {
            Id = id;
            AgeGroupId = ageGroupId;
            AgeGroup = ageGroup;
            MaxWeight = maxWeight;
            WeightName = weightName;
            Gender = gender;
        }
        
        protected WeightCategorie() {}
        
        public Guid AgeGroupId { get; private set; }
        public  AgeGroup AgeGroup { get; private set; }
        public int MaxWeight { get; private set; }
        public  string WeightName { get; private set; }
        public Gender Gender { get; private set; }
        
        public IReadOnlyCollection<Fighter> Fighters { get; protected set; }
        public IReadOnlyCollection<Bracket> Brackets { get; protected set; }

    }
}
