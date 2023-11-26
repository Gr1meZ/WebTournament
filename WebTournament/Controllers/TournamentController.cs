using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;
using WebTournament.Models.Helpers;
using WebTournament.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebTournament.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TournamentController : Controller
    {
        private readonly ITournamentService _tournamentService;

        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddIndex()
        {
            return View();
        }

        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> EditIndex(Guid id)
        {
            return View(await _tournamentService.GetTournamentAsync(id));
        }
        
        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> Winners(Guid id)
        {
            return View(await _tournamentService.GetTournamentResultsAsync(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> List([FromBody] DtQuery query)
        {
            return Json(await _tournamentService.TournamentsListAsync(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(TournamentViewModel tournamentViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            await _tournamentService.AddTournamentAsync(tournamentViewModel);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(TournamentViewModel tournamentViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _tournamentService.EditTournamentAsync(tournamentViewModel);
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _tournamentService.DeleteTournamentAsync(id);
            return Ok();
        }

        public async Task<IActionResult> Select2Tournaments([FromForm] Select2Request request)
        {
            return Ok(await _tournamentService.GetSelect2TournamentsAsync(request));
        }
    }
}
