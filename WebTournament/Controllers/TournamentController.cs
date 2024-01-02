using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebTournament.Application.Common;
using WebTournament.Application.DTO;
using WebTournament.Application.Tournament.CreateTournament;
using WebTournament.Application.Tournament.GetTournament;
using WebTournament.Application.Tournament.GetTournamentList;
using WebTournament.Application.Tournament.GetTournamentResults;
using WebTournament.Application.Tournament.RemoveTournament;
using WebTournament.Application.Tournament.UpdateTournament;

namespace WebTournament.Presentation.MVC.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public TournamentController(IMediator mediator, IMapper mapper)
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
            return View(await _mediator.Send(new GetTournamentQuery(id)));
        }
        
        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> Winners(Guid id)
        {
            return View(await _mediator.Send(new GetTournamentResultsQuery(id)));
        }
        
        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetTournamentListQuery query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(TournamentDto tournamentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage).ToList());
            
            var command = _mapper.Map<CreateTournamentCommand>(tournamentDto);
            await _mediator.Send(command);
            return CreatedAtAction(nameof(EditIndex), new { id = tournamentDto.Id }, tournamentDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(TournamentDto tournamentDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
           
            await _mediator.Send(_mapper.Map<UpdateTournamentCommand>(tournamentDto));
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _mediator.Send(new RemoveTournamentCommand(id));
            return NoContent();
        }
        
    }
}
