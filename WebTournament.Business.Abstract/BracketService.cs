using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract;

public interface IBracketService
{
    public Task GenerateBrackets(BracketViewModel bracketViewModel);
    Task DistributeAllPlayers(Guid tournamentId);
    public Task DeleteAllBrackets(Guid tournamentId);
    Task<PagedResponse<BracketViewModel[]>> BracketsList(PagedRequest request, Guid tournamentId);
    Task DeleteBracket(Guid bracketId);
}