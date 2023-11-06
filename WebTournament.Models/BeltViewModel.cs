using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models
{
    public class BeltViewModel
    {
        public Guid Id { get; set; }
        public int BeltNumber { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }
}
