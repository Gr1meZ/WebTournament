using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Models.Helpers
{
    public class DtQuery : PagedRequest
    {
        public int Draw { get; set; }
    }
}
