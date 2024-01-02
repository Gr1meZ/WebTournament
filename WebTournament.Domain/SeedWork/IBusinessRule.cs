namespace WebTournament.Domain.SeedWork;

public interface IBusinessRule
{
    Task<bool> IsBrokenAsync();

    string Message { get; }
}