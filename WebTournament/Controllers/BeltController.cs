using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Application.Belt.CreateBelt;
using WebTournament.Application.Belt.GetBelt;
using WebTournament.Application.Belt.GetBeltList;
using WebTournament.Application.Belt.RemoveBelt;
using WebTournament.Application.Belt.UpdateBelt;
using WebTournament.Application.DTO;
using WebTournament.Application.Select2.Queries;

namespace WebTournament.Presentation.MVC.Controllers
{
    public class BeltController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public BeltController(IMediator mediator, IMapper mapper)
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
            return View(await _mediator.Send(new GetBeltQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetBeltListQuery query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(BeltDto beltDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            var command = _mapper.Map<CreateBeltCommand>(beltDto);
            await _mediator.Send(command);
            return CreatedAtAction(nameof(EditIndex), new { id = beltDto.Id }, beltDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(BeltDto beltDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            await _mediator.Send(_mapper.Map<UpdateBeltCommand>(beltDto));
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _mediator.Send(new RemoveBeltCommand(id));
            return NoContent();
        }

        public async Task<IActionResult> Select2Belts([FromForm] Select2BeltQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}