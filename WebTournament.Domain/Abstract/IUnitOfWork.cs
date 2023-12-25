namespace WebTournament.Domain.Abstract;

public interface IUnitOfWork: IDisposable
{
    bool Commit();
}