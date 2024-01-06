namespace WebTournament.Application.Trainer;

public class TrainerResponse
{
    public Guid Id { get; set; }

    public Guid? ClubId { get; set; }

    public string ClubName { get; init; } = string.Empty;

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }

    public string Phone { get; set; }
}