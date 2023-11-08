using DataAccess.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models
{
    public class FighterViewModel
    {
  
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public Guid WeightCategorieId { get; set; }
        public Guid BeltId { get; set; }
        public Guid TrainerId { get; set; }
        public string TournamentName { get; set; }
        public string WeightCategorieName { get; set; }
        public string BeltName { get; set; }
        public string TrainerName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
    }
}
