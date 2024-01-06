using WebTournament.Application.SeedPaging;

namespace WebTournament.Presentation.MVC.Models
{
    public class DtQuery : PagedRequest
    {
        public int Draw { get; set; }
    }
}
