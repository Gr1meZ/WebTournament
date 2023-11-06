using DataAccess.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Domain.Models
{
    public class Fighter : BaseEntity
    {
        public Guid TournamentId { get; set; }
        public Guid WeightCategorieId { get; set; }
        public Guid BeltId { get; set; }
        public Guid TrainerId { get; set; }
        public  string Name { get; set; }
        public  string Surname { get; set; }
        public int Age { get; set; }
        public  string Country { get; set; }
        public  string City { get; set; }
        public Gender Gender { get; set; }

        public  Tournament Tournament { get; set;}
        public  WeightCategorie WeightCategorie { get; set;}
        public  Belt Belt { get; set;}  
        public  Trainer Trainer { get; set;}    

    }
}
