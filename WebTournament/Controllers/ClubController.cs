// using Microsoft.AspNetCore.Mvc;
// using WebTournament.Business.Abstract;
// using WebTournament.Models.Helpers;
// using WebTournament.Models;
// using Microsoft.AspNetCore.Authorization;
//
// namespace WebTournament.Presentation.MVC.Controllers
// {
//     [Authorize(Roles = "Admin")]
//     public class ClubController : Controller
//     {
//         private readonly IClubService _clubService;
//
//         public ClubController(IClubService clubService)
//         {
//             _clubService = clubService;
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
//             return View(await _clubService.GetClubAsync(id));
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> List([FromBody] DtQuery query)
//         {
//             return Json(await _clubService.ClubListAsync(query));
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> AddModel(ClubDto clubDto)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
//             await _clubService.AddClubAsync(clubDto);
//             return Ok();
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> EditModel(ClubDto clubDto)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
//
//             await _clubService.EditClubAsync(clubDto);
//             return Ok();
//         }
//
//         [HttpDelete("[controller]/{id}")]
//         public async Task<IActionResult> DeleteModel(Guid id)
//         {
//             await _clubService.DeleteClubAsync(id);
//             return Ok();
//         }
//
//         public async Task<IActionResult> Select2Clubs([FromForm] Select2Request request)
//         {
//             return Ok(await _clubService.GetSelect2ClubsAsync(request));
//         }
//     }
// }