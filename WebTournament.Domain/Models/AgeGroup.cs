using WebTournament.Domain.Core.Models;

namespace WebTournament.Domain.Models
{
    public class AgeGroup : Entity
    {
        public AgeGroup(Guid id, string name, int minAge, int maxAge)
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

        public IReadOnlyCollection<WeightCategorie> WeightCategories { get; protected set; }
    }
}
