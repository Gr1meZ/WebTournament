using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;
using WebTournament.Models.Helpers;
using WebTournament.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebTournament.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AgeGroupController : Controller
    {
        private readonly IAgeGroupService _ageGroupService;

        public AgeGroupController(IAgeGroupService ageGroupService)
        {
            _ageGroupService = ageGroupService;
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
            return View(await _ageGroupService.GetAgeGroup(id));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] DtQuery query)
        {
            return Json(await _ageGroupService.AgeGroupList(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(AgeGroupViewModel ageGroupViewModel)
        {
            if (!ModelState.IsValid) return View("AddIndex");
            await _ageGroupService.AddAgeGroup(ageGroupViewModel);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(AgeGroupViewModel ageGroupViewModel)
        {
            if (!ModelState.IsValid) return View("EditIndex");

            await _ageGroupService.EditAgeGroup(ageGroupViewModel);
            return RedirectToAction("Index");
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModelt(Guid id)
        {
            await _ageGroupService.DeleteAgeGroup(id);
            return Ok();
        }

        public async Task<IActionResult> Select2AgeGroups([FromForm] Select2Request request)
        {
            return Ok(await _ageGroupService.GetAutoCompleteAgeGroups(request));
        }
    }
}
