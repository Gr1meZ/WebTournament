namespace WebTournament.Presentation.MVC.Models
{
    public class ErrorDto
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}