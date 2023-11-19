using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Domain.Models
{
    public class Tournament : BaseEntity
    {
        public  string Name { get; set; }
        public DateTime StartDate { get; set; }
        public  string Address { get; set; }
        public ICollection<Fighter> Fighters { get; set; }
        public ICollection<Bracket> Brackets { get; set; }
    }
}
