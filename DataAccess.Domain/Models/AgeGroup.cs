using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Domain.Models
{
    public class AgeGroup : BaseEntity
    {
        public  string Name { get; set; }
        public  int MinAge { get; set; }
        public  int MaxAge { get; set;}

        public ICollection<WeightCategorie> WeightCategories { get; set; }
    }
}
