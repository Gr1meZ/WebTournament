using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;
using WebTournament.Models.Helpers;
using WebTournament.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebTournament.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
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
            return View(await _trainerService.GetTrainer(id));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] DtQuery query)
        {
            return Json(await _trainerService.TrainersList(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(TrainerViewModel trainerViewModel)
        {
            if (!ModelState.IsValid) return View();
            await _trainerService.AddTrainer(trainerViewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(TrainerViewModel trainerViewModel)
        {
            if (!ModelState.IsValid) return View("EditIndex");

            await _trainerService.EditTrainer(trainerViewModel);
            return RedirectToAction("Index");
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModelt(Guid id)
        {
            await _trainerService.DeleteTrainer(id);
            return Ok();
        }
    }
}
