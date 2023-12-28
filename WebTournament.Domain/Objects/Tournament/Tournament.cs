using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Tournament
{
    public class Tournament : Entity
    {
        public Tournament(Guid id, string name, DateTime startDate, string address)
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
    }
}
