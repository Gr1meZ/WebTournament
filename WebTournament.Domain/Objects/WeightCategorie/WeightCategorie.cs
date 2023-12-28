using WebTournament.Domain.Enums;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.WeightCategorie
{
    public class WeightCategorie : Entity
    {
        public WeightCategorie(Guid id, Guid ageGroupId, AgeGroup.AgeGroup ageGroup, int maxWeight, string weightName, Gender gender)
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
        public  AgeGroup.AgeGroup AgeGroup { get; private set; }
        public int MaxWeight { get; private set; }
        public  string WeightName { get; private set; }
        public Gender Gender { get; private set; }
        
        public IReadOnlyCollection<Fighter.Fighter> Fighters { get; protected set; }
        public IReadOnlyCollection<Bracket.Bracket> Brackets { get; protected set; }

    }
}
