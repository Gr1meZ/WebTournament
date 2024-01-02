using WebTournament.Domain.Enums;
using WebTournament.Domain.Extensions;
using WebTournament.Domain.Objects.WeightCategorie.Rules;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.WeightCategorie
{
    public class WeightCategorie : Entity
    {
        private WeightCategorie(Guid id, Guid ageGroupId, int maxWeight, string weightName, Gender gender)
        {
            Id = id;
            AgeGroupId = ageGroupId;
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

        public static async Task<WeightCategorie> CreateAsync(Guid ageGroupId, int maxWeight, string weightName,
            Gender gender, IWeightCategorieRepository weightCategorieRepository)
        {
            await CheckRuleAsync(
                new WeightCategorieMustBeUniqueRule(weightCategorieRepository, maxWeight, gender.MapToString(), ageGroupId));
            
            return new WeightCategorie(Guid.NewGuid(), ageGroupId, maxWeight, weightName, gender);
        }

        public void Change(Guid ageGroupId, int maxWeight, string weightName,
            Gender gender)
        {
            AgeGroupId = ageGroupId;
            MaxWeight = maxWeight;
            WeightName = weightName;
            Gender = gender;
        }
    }
}
