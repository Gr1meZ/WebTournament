

namespace DataAccess.Domain.Models
{
    public class Belt : BaseEntity
    {
        public  int BeltNumber {  get; set; }
        public string ShortName { get; set;}
        public string FullName { get; set;}
        public ICollection<Fighter> Fighters { get; set; }

    }
}
