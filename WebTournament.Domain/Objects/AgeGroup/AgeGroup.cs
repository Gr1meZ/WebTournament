using WebTournament.Domain.Objects.AgeGroup.Rules;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.AgeGroup
{
    public class AgeGroup : Entity
    {
        private AgeGroup(Guid id, string name, int minAge, int maxAge)
        {
            Id = id;
            Name = name;
            MinAge = minAge;
            MaxAge = maxAge;
        }

        protected AgeGroup() {}
        
        public  string Name { get; private set; }
        public  int MinAge { get; private set; }
        public  int MaxAge { get; private set;}

         public IReadOnlyCollection<WeightCategorie.WeightCategorie> WeightCategories { get; protected set; }

         public static async Task<AgeGroup> CreateAsync(Guid id, string name, int? minAge, int? maxAge, IAgeGroupRepository ageGroupRepository)
         {
             await CheckRuleAsync(new AgeGroupMustBeUniqueRule(ageGroupRepository, minAge, maxAge));
             return new AgeGroup() { Id = id, MaxAge = maxAge.Value, MinAge = minAge.Value, Name = name };
         }
         
         public void Change(string name, int? minAge, int? maxAge)
         {
             Name = name;
             MinAge = minAge.Value;
             MaxAge = maxAge.Value;
         }
    }
}
