using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Business.Abstract;

public interface IBracketService
{
    public Task GenerateBracketsAsync(BracketViewModel bracketViewModel);
    Task SaveStateAsync(BracketState bracketState);
    Task<BracketState> GetBracketAsync(Guid bracketId);
    Task DistributeAllPlayersAsync(Guid tournamentId);
    public Task DeleteAllBracketsAsync(Guid tournamentId);
    Task<PagedResponse<BracketViewModel[]>> BracketsListAsync(PagedRequest request, Guid tournamentId);
    Task DeleteBracketAsync(Guid bracketId);
    Task<Select2Response> GetSelect2BracketFightersAsync(Select2Request request,  Guid bracketId);
}