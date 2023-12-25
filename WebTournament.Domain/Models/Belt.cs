using WebTournament.Domain.Core.Models;

namespace WebTournament.Domain.Models
{
    public class Belt : Entity
    {
        public Belt(Guid id, int beltNumber, string shortName, string fullName)
        {
            Id = id;
            BeltNumber = beltNumber;
            ShortName = shortName;
            FullName = fullName;
        }
        
        protected Belt(){}
        
        public  int BeltNumber {  get; private set; }
        public string ShortName { get; private set;}
        public string FullName { get; private set;}
        
        public IReadOnlyCollection<Fighter> Fighters { get;  protected set; }

    }
}
