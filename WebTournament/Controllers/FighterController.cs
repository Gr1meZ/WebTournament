using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;
using WebTournament.Models.Helpers;
using WebTournament.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebTournament.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FighterController : Controller
    {
        private readonly IFighterService _fighterService;
        private readonly ITournamentService _tournamentService;

        public FighterController(IFighterService fighterService, ITournamentService tournamentService)
        {
            _fighterService = fighterService;
            _tournamentService = tournamentService;
        }

        [HttpGet("[controller]/{tournamentId}")]
        public async Task<IActionResult> Index(Guid tournamentId)
        {
            var tournament = await _tournamentService.GetTournamentAsync(tournamentId);
            ViewData["Tournament"] = tournament.Name;
            return View(tournamentId);
        }

        [HttpGet("[controller]/[action]/{tournamentId}")]
        public IActionResult AddIndex(Guid tournamentId)
        {
            return View(new FighterViewModel() {TournamentId = tournamentId});
        }

        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> EditIndex(Guid id)
        {
            return View(await _fighterService.GetFighterAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] DtQuery query, Guid tournamentId)
        {
            return Json(await _fighterService.FightersListAsync(query, tournamentId));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(FighterViewModel fighterViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            await _fighterService.AddFighterAsync(fighterViewModel);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(FighterViewModel fighterViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _fighterService.EditFighterAsync(fighterViewModel);
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _fighterService.DeleteFighterAsync(id);
            return Ok();
        }
        
        [HttpDelete("[controller]/[action]/{tournamentId}")]
        public async Task<IActionResult> DeleteAll(Guid tournamentId)
        {
            await _fighterService.DeleteAllFightersAsync(tournamentId);
            return Ok();
        }
    }
}

