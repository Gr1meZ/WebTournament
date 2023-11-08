using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models
{
    public class WeightCategorieViewModel
    {
        public Guid Id { get; set; }
        public Guid AgeGroupId { get; set; }
        public string AgeGroupName { get; set; }
        public int MaxWeight { get; set; }
        public string WeightName { get; set; }
    }
}
