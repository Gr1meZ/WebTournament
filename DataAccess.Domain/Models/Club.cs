using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Domain.Models
{
    public class Club : BaseEntity
    {
        public  string Name { get; set; }
        public ICollection<Trainer> Trainers { get; set; }
    }
}
