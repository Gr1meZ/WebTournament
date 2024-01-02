using WebTournament.Domain.Objects.Club.Rules;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Club
{
    public class Club : Entity
    {
        private Club(string name)
        {
            Name = name;
        }
        
        protected Club() {}
        public  string Name { get; private set; }
        public IReadOnlyCollection<Trainer.Trainer> Trainers { get;  protected set; }

        public static async Task<Club> CreateAsync(string name, IClubRepository clubRepository)
        {
            await CheckRuleAsync(new ClubMustBeUniqueRule(clubRepository, name));
            return new Club() { Id = Guid.NewGuid(), Name = name};
        }

        public void Change(string name) => Name = name;

    }
}
