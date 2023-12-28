using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroupList;
using WebTournament.Application.AgeGroup.RemoveAgeGroup;
using WebTournament.Application.AgeGroup.UpdateAgeGroup;
using WebTournament.Application.Common;
using WebTournament.Application.Select2.Queries;

namespace WebTournament.Presentation.MVC.Controllers
{
    public class AgeGroupController : Controller
    {
        private readonly IMediator _mediator;

        public AgeGroupController(IMediator mediator)
        {
            _mediator = mediator;
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
            return View(await _mediator.Send(new GetAgeGroupQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetAgeGroupListQuery request)
        {
            return Json(await _mediator.Send(request));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(CreateAgeGroupCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(UpdateAgeGroupCommand command)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _mediator.Send(new RemoveAgeGroupCommand(id));
            return Ok();
        }

        public async Task<IActionResult> Select2AgeGroups([FromForm] Select2AgeGroupsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
