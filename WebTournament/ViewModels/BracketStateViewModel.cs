namespace WebTournament.Presentation.MVC.ViewModels;

public class BracketStateViewModel
{
    public Guid Id { get; set; }
    public string State { get; set; }
    public string CategorieName { get; set; }
    public List<Guid> Winners { get; set; }
}