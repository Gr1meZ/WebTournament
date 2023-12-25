using WebTournament.Domain.Core.Models;

namespace WebTournament.Domain.Abstract;

public interface IUnitOfWork: IDisposable
{
    Task Commit();

}