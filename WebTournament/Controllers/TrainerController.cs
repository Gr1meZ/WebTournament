// using Microsoft.AspNetCore.Mvc;
// using WebTournament.Business.Abstract;
// using WebTournament.Models.Helpers;
// using WebTournament.Models;
// using Microsoft.AspNetCore.Authorization;
//
// namespace WebTournament.Presentation.MVC.Controllers
// {
//     [Authorize(Roles = "Admin")]
//     public class TrainerController : Controller
//     {
//         private readonly ITrainerService _trainerService;
//
//         public TrainerController(ITrainerService trainerService)
//         {
//             _trainerService = trainerService;
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
//             return View(await _trainerService.GetTrainerAsync(id));
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> List([FromBody] DtQuery query)
//         {
//             return Json(await _trainerService.TrainersListAsync(query));
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> AddModel(TrainerDto trainerDto)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
//             await _trainerService.AddTrainerAsync(trainerDto);
//             return Ok();
//         }
//
//         [HttpPost]
//         public async Task<IActionResult> EditModel(TrainerDto trainerDto)
//         {
//             if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
//
//             await _trainerService.EditTrainerAsync(trainerDto);
//             return Ok();
//         }
//
//         [HttpDelete("[controller]/{id}")]
//         public async Task<IActionResult> DeleteModel(Guid id)
//         {
//             await _trainerService.DeleteTrainerAsync(id);
//             return Ok();
//         }
//
//         public async Task<IActionResult> Select2Trainers([FromForm] Select2Request request)
//         {
//             return Ok(await _trainerService.GetAutoCompleteTrainersAsync(request));
//         }
//     }
// }
