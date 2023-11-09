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

        public FighterController(IFighterService fighterSErvice)
        {
            _fighterService = fighterSErvice;
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
            return View(await _fighterService.GetFighter(id));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] DtQuery query)
        {
            return Json(await _fighterService.FightersList(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(FighterViewModel fighterViewModel)
        {
            if (!ModelState.IsValid) return View();
            await _fighterService.AddFighter(fighterViewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(FighterViewModel fighterViewModel)
        {
            if (!ModelState.IsValid) return View("EditIndex");

            await _fighterService.EditFighter(fighterViewModel);
            return RedirectToAction("Index");
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModelt(Guid id)
        {
            await _fighterService.DeleteFighter(id);
            return Ok();
        }
    }
}

