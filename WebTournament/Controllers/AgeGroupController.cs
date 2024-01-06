using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Application.AgeGroup.CreateAgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroup;
using WebTournament.Application.AgeGroup.GetAgeGroupList;
using WebTournament.Application.AgeGroup.RemoveAgeGroup;
using WebTournament.Application.AgeGroup.UpdateAgeGroup;
using WebTournament.Application.Select2.Queries;
using WebTournament.Presentation.MVC.ViewModels;

namespace WebTournament.Presentation.MVC.Controllers
{
    public class AgeGroupController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public AgeGroupController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
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
            var response = await _mediator.Send(new GetAgeGroupQuery(id));
            return View(_mapper.Map<AgeGroupViewModel>(response));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetAgeGroupListQuery request)
        {
            return Json(await _mediator.Send(request));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(AgeGroupViewModel ageGroupViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            await _mediator.Send(_mapper.Map<CreateAgeGroupCommand>(ageGroupViewModel));
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(AgeGroupViewModel ageGroupViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            await _mediator.Send(_mapper.Map<UpdateAgeGroupCommand>(ageGroupViewModel));
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _mediator.Send(new RemoveAgeGroupCommand(id));
            return Ok();
        }

        public async Task<IActionResult> Select2AgeGroups([FromForm]Select2AgeGroupsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}
