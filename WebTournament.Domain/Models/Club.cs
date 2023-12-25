using WebTournament.Domain.Core.Models;

namespace WebTournament.Domain.Models
{
    public class Club : Entity
    {
        public Club(string name)
        {
            Name = name;
        }
        protected Club() {}
        public  string Name { get; private set; }
        public ICollection<Trainer> Trainers { get; protected set; }
    }
}
