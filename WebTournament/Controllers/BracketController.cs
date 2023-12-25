using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.Presentation.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class BracketController : Controller
{
    private readonly IBracketService _bracketService;

    public BracketController(IBracketService bracketService)
    {
        _bracketService = bracketService;
    }
    
    [HttpGet("[controller]/[action]/{tournamentId}")]
    public IActionResult TournamentBrackets(Guid tournamentId)
    {
        return View(tournamentId);
    }
    
    [HttpPost]
    public async Task<IActionResult> List([FromBody] DtQuery query, Guid tournamentId)
    {
        return Json(await _bracketService.BracketsListAsync(query, tournamentId));
    }
    
    public IActionResult AddIndex(Guid tournamentId)
    {
        return View(new BracketViewModel()
        {
            TournamentId = tournamentId
        });
    }
    
    [HttpDelete("[controller]/{id}")]
    public async Task<IActionResult> DeleteModel(Guid id)
    {
        await _bracketService.DeleteBracketAsync(id);
        return Ok();
    }
    
    [HttpDelete("[controller]/[action]/{tournamentId}")]
    public async Task<IActionResult> DeleteAllBrackets(Guid tournamentId)
    {
        await _bracketService.DeleteAllBracketsAsync(tournamentId);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddModel(BracketViewModel bracketViewModel)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
        await _bracketService.GenerateBracketsAsync(bracketViewModel);
        return Ok();
    }
    
    [HttpPost("[controller]/[action]/{tournamentId}")]
    public async Task<IActionResult> DrawFighters(Guid tournamentId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
        await _bracketService.DistributeAllPlayersAsync(tournamentId);
        return Ok();
    }

    [HttpGet("[controller]/[action]/{bracketId}")]
    public async Task<IActionResult> GetBracket(Guid bracketId)
    {
        var bracket = await _bracketService.GetBracketAsync(bracketId);
        ViewData["Winners"] = JsonConvert.SerializeObject(bracket.Winners);
        return View("Bracket", bracket);
    }

    [HttpPost]
    public async Task<IActionResult> SaveState(BracketState bracketState)
    {
        await _bracketService.SaveStateAsync(bracketState);
        return Ok();
    }
    
    [HttpPost("[controller]/[action]/{bracketId}")]
    public async Task<IActionResult> Select2BracketFighters([FromForm]Select2Request request, Guid bracketId)
    {
        return Ok(await _bracketService.GetSelect2BracketFightersAsync(request, bracketId));
    }
}