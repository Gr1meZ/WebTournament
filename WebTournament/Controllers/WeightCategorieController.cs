using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;
using WebTournament.Business.Services;
using WebTournament.Models;
using WebTournament.Models.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebTournament.WebApp.Controllers
{

    [Authorize(Roles = "Admin")]
    public class WeightCategorieController : Controller
    {
        private readonly IWeightCategorieService _weightCategorieService;

        public WeightCategorieController(IWeightCategorieService weightCategorieService) {
            _weightCategorieService = weightCategorieService;
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
            return View(await _weightCategorieService.GetWeightCategorie(id));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] DtQuery query)
        {
            return Json(await _weightCategorieService.WeightCategoriesList(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(WeightCategorieViewModel weightCategorie)
        {
            if (!ModelState.IsValid) return View();
            await _weightCategorieService.AddWeightCategorie(weightCategorie);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(WeightCategorieViewModel weightCategorie)
        {
            if (!ModelState.IsValid) return View("EditIndex");

            await _weightCategorieService.EditWeightCategorie(weightCategorie);
            return RedirectToAction("Index");
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModelt(Guid id)
        {
            await _weightCategorieService.DeleteWeightCategorie(id);
            return Ok();
        }

        public async Task<IActionResult> Select2WeightCategories([FromForm] Select2Request request)
        {
            return Ok(await _weightCategorieService.GetAutoCompleteWeightCategories(request));
        }
    }
}
