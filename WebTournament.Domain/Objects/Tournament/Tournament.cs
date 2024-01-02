using WebTournament.Domain.Objects.Tournament.Rules;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Tournament
{
    public class Tournament : Entity
    {
        private Tournament(Guid id, string name, DateTime startDate, string address)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
            Address = address;
        }
        
        protected Tournament() {}
        
        public  string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public  string Address { get; private set; }
        
        public IReadOnlyCollection<Fighter.Fighter> Fighters { get; protected set; }
        public IReadOnlyCollection<Bracket.Bracket> Brackets { get; protected set; }

        public static async Task<Tournament> CreateAsync(string name, DateTime startDate, string address, ITournamentRepository tournamentRepository)
        {
            await CheckRuleAsync(new TournamentMustBeUniqueRule(address, name, tournamentRepository));
            return new Tournament(Guid.NewGuid(), name, startDate, address);
        }

        public void Change(string name, DateTime startDate, string address)
        {
            Name = name;
            StartDate = startDate;
            Address = address;
        }
    }
}
