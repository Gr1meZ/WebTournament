using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;
using WebTournament.Models;
using WebTournament.Models.Helpers;

namespace WebTournament.WebApp.Controllers;

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
        return Json(await _bracketService.BracketsList(query, tournamentId));
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
        await _bracketService.DeleteBracket(id);
        return Ok();
    }
    
    [HttpDelete("[controller]/[action]/{tournamentId}")]
    public async Task<IActionResult> DeleteAllBrackets(Guid tournamentId)
    {
        await _bracketService.DeleteAllBrackets(tournamentId);
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddModel(BracketViewModel bracketViewModel)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
        await _bracketService.GenerateBrackets(bracketViewModel);
        return Ok();
    }
}