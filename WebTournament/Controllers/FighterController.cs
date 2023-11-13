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

        public FighterController(IFighterService fighterSErvice, ITournamentService tournamentService)
        {
            _fighterService = fighterSErvice;
            _tournamentService = tournamentService;
        }

        [HttpGet("[controller]/{tournamentId}")]
        public async Task<IActionResult> Index(Guid tournamentId)
        {
            var tournament = await _tournamentService.GetTournament(tournamentId);
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
            return View(await _fighterService.GetFighter(id));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] DtQuery query, Guid tournamentId)
        {
            return Json(await _fighterService.FightersList(query, tournamentId));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(FighterViewModel fighterViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            await _fighterService.AddFighter(fighterViewModel);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(FighterViewModel fighterViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _fighterService.EditFighter(fighterViewModel);
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModelt(Guid id)
        {
            await _fighterService.DeleteFighter(id);
            return Ok();
        }
    }
}

