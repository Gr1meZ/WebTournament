// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using WebTournament.Business.Abstract;
// using WebTournament.Models;
// using WebTournament.Models.Helpers;
//
// namespace WebTournament.Presentation.MVC.Controllers
// {
//
//     [Authorize(Roles = "Admin")]
//     public class WeightCategorieController : Controller
//     {
//         private readonly IWeightCategorieService _weightCategorieService;
//
//         public WeightCategorieController(IWeightCategorieService weightCategorieService) {
//             _weightCategorieService = weightCategorieService;
//         }
//
//         public IActionResult Index()
//         {
//             return View();
//         }
//
//         public IActionResult AddIndex()
//         {
//             return View();
//         }
//
//         [HttpGet("[controller]/{id}/[action]")]
//         public async Task<IActionResult> EditIndex(Guid id)
//         {
//             return View(await _weightCategorieService.GetWeightCategorieAsync(id));
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> List([FromBody] DtQuery query)
//         {
//             return Json(await _weightCategorieService.WeightCategoriesListAsync(query));
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> AddModel(WeightCategorieDto weightCategorie)
//         {
//             if (!ModelState.IsValid || !weightCategorie.AgeGroupId.HasValue) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
//             await _weightCategorieService.AddWeightCategorieAsync(weightCategorie);
//             return Ok();
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> EditModel(WeightCategorieDto weightCategorie)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
//
//             await _weightCategorieService.EditWeightCategorieAsync(weightCategorie);
//             return Ok();
//         }
//
//         [HttpDelete("[controller]/{id}")]
//         public async Task<IActionResult> DeleteModel(Guid id)
//         {
//             await _weightCategorieService.DeleteWeightCategorieAsync(id);
//             return Ok();
//         }
//
//         public async Task<IActionResult> Select2WeightCategories([FromForm] Select2Request request)
//         {
//             return Ok(await _weightCategorieService.GetSelect2WeightCategoriesAsync(request));
//         }
//     }
// }
