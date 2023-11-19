namespace DataAccess.Domain.Models;

public class Bracket : BaseEntity
{
    public Guid WeightCategorieId { get; set; }
    public Guid TournamentId { get; set; }
    public Guid[] Division { get; set; }
    public string State { get; set; }
    public WeightCategorie WeightCategorie { get; set; }
    public Tournament Tournament { get; set; }
    public ICollection<Fighter> Fighters { get; set; }
    
}