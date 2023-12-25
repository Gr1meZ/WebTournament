// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace WebTournament.Presentation.MVC.Controllers
// {
//     [Authorize(Roles = "Admin")]
//     public class AgeGroupController : Controller
//     {
//         private readonly IAgeGroupService _ageGroupService;
//
//         public AgeGroupController(IAgeGroupService ageGroupService)
//         {
//             _ageGroupService = ageGroupService;
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
//             return View(await _ageGroupService.GetAgeGroupAsync(id));
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> List([FromBody] DtQuery query)
//         {
//             return Json(await _ageGroupService.AgeGroupListAsync(query));
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> AddModel(AgeGroupDto ageGroupDto)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
//             await _ageGroupService.AddAgeGroupAsync(ageGroupDto);
//             return Ok();
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> EditModel(AgeGroupDto ageGroupDto)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
//
//             await _ageGroupService.EditAgeGroupAsync(ageGroupDto);
//             return Ok();
//         }
//
//         [HttpDelete("[controller]/{id}")]
//         public async Task<IActionResult> DeleteModel(Guid id)
//         {
//             await _ageGroupService.DeleteAgeGroupAsync(id);
//             return Ok();
//         }
//
//         public async Task<IActionResult> Select2AgeGroups([FromForm] Select2Request request)
//         {
//             return Ok(await _ageGroupService.GetSelect2AgeGroupsAsync(request));
//         }
//     }
// }
