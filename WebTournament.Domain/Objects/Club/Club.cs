using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Club
{
    public class Club : Entity
    {
        public Club(string name)
        {
            Name = name;
        }
        protected Club() {}
        public  string Name { get; private set; }
        public IReadOnlyCollection<Trainer.Trainer> Trainers { get;  protected set; }
    }
}
