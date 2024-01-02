using WebTournament.Domain.Objects.Belt.Rules;
using WebTournament.Domain.SeedWork;

namespace WebTournament.Domain.Objects.Belt
{
    public class Belt : Entity
    {
        private Belt(Guid id, int beltNumber, string shortName, string fullName)
        {
            Id = id;
            BeltNumber = beltNumber;
            ShortName = shortName;
            FullName = fullName;
        }
        
        protected Belt(){}
        
        public int BeltNumber {  get; private set; }
        public string ShortName { get; private set;}
        public string FullName { get; private set;}
        
        public IReadOnlyCollection<Fighter.Fighter> Fighters { get;  protected set; }
        
        public static async Task<Belt> CreateAsync(int beltNumber, string shortName, string fullName, IBeltRepository beltRepository)
        {
            await CheckRuleAsync(new BeltMustBeUniqueRule(beltRepository, beltNumber, shortName));
            return new Belt() { Id = Guid.NewGuid(), BeltNumber = beltNumber, FullName = fullName, ShortName = shortName};
        }
         
        public void Change(int beltNumber, string shortName, string fullName)
        {
            BeltNumber = beltNumber;
            ShortName = shortName;
            FullName = fullName;
        }
    }
}
