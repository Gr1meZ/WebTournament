using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;
using WebTournament.Models.Helpers;
using WebTournament.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebTournament.Presentation.MVC.Controllers
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
            return View(await _beltService.GetBeltAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] DtQuery query)
        {
            return Json(await _beltService.BeltListAsync(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(BeltViewModel beltViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            await _beltService.AddBeltAsync(beltViewModel);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(BeltViewModel beltViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _beltService.EditBeltAsync(beltViewModel);
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _beltService.DeleteBeltAsync(id);
            return Ok();
        }

        public async Task<IActionResult> Select2Belts([FromForm] Select2Request request)
        {
            return Ok(await _beltService.GetSelect2BeltsAsync(request));
        }
    }
}