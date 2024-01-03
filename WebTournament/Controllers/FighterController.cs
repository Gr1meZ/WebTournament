using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Application.DTO;
using WebTournament.Application.Fighter.CreateFighter;
using WebTournament.Application.Fighter.CreateFightersFromExcel;
using WebTournament.Application.Fighter.GetFighter;
using WebTournament.Application.Fighter.GetFighterList;
using WebTournament.Application.Fighter.RemoveAllFighters;
using WebTournament.Application.Fighter.RemoveFighter;
using WebTournament.Application.Fighter.UpdateFighter;
using WebTournament.Application.Tournament.GetTournament;

namespace WebTournament.Presentation.MVC.Controllers
{
    public class FighterController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FighterController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("[controller]/{tournamentId}")]
        public async Task<IActionResult> Index(Guid tournamentId)
        {
            var tournament = await _mediator.Send(new GetTournamentQuery(tournamentId));
            ViewData["Tournament"] = tournament.Name;
            return View(tournamentId);
        }

        [HttpGet("[controller]/[action]/{tournamentId}")]
        public IActionResult AddIndex(Guid tournamentId)
        {
            return View(new FighterDto() {TournamentId = tournamentId});
        }

        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> EditIndex(Guid id)
        {
            return View(await _mediator.Send(new GetFighterQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetFighterListQuery query, Guid tournamentId)
        {
            query.TournamentId = tournamentId;
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(FighterDto fighterDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            var command = _mapper.Map<CreateFighterCommand>(fighterDto);
            await _mediator.Send(command);
            
            return CreatedAtAction(nameof(EditIndex), new { id = fighterDto.Id }, fighterDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(FighterDto fighterDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _mediator.Send(_mapper.Map<UpdateFighterCommand>(fighterDto));
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _mediator.Send(new RemoveFighterCommand(id));
            return NoContent();
        }
        
        [HttpDelete("[controller]/[action]/{tournamentId}")]
        public async Task<IActionResult> DeleteAll(Guid tournamentId)
        {
            await _mediator.Send(new RemoveAllFightersCommand(tournamentId));
            return NoContent();
        }
        
        [HttpPost]
        public async Task<IActionResult> GenerateFromExcel([FromQuery]Guid tournamentId, CancellationToken cancellationToken)
        {
            var formFile = Request.Form.Files[0];
            await _mediator.Send(new CreateFightersFromExcelCommand(tournamentId, formFile), cancellationToken);
            return Ok();
        }
    }
}

