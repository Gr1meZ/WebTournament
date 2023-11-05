using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Domain.Models
{
    public class Trainer : BaseEntity
    {
        public Guid ClubId { get; set; }
        public  Club Club { get; set; }
        public  string Name { get; set; }
        public  string Surname { get; set; }
        public  string Patronymic { get; set; }
        public  string Phone { get; set; }

        public ICollection<Fighter> Fighters { get; set; }

    }
}
