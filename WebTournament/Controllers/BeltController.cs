using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;
using WebTournament.Models.Helpers;
using WebTournament.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebTournament.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BeltController : Controller
    {
        private readonly IBeltService _beltService;

        public BeltController(IBeltService beltService)
        {
            _beltService = beltService;
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
            return View(await _beltService.GetBelt(id));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] DtQuery query)
        {
            return Json(await _beltService.BeltList(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(BeltViewModel beltViewModel)
        {
            if (!ModelState.IsValid) return View();
            await _beltService.AddBelt(beltViewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(BeltViewModel beltViewModel)
        {
            if (!ModelState.IsValid) return View("EditIndex");

            await _beltService.EditBelt(beltViewModel);
            return RedirectToAction("Index");
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModelt(Guid id)
        {
            await _beltService.DeleteBelt(id);
            return Ok();
        }
    }
}