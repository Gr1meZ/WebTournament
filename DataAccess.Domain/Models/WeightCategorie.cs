using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Domain.Models
{
    public class WeightCategorie : BaseEntity
    {
        public Guid AgeGroupId { get; set; }
        public  AgeGroup AgeGroup { get; set; }
        public int MaxWeight { get; set; }
        public  string WeightName { get; set; }
        public ICollection<Fighter> Fighters { get; set; }


    }
}
