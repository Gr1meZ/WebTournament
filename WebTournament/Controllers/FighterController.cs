using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Application.Fighter.CreateFighter;
using WebTournament.Application.Fighter.CreateFightersFromExcel;
using WebTournament.Application.Fighter.GetFighter;
using WebTournament.Application.Fighter.GetFighterList;
using WebTournament.Application.Fighter.RemoveAllFighters;
using WebTournament.Application.Fighter.RemoveFighter;
using WebTournament.Application.Fighter.UpdateFighter;
using WebTournament.Application.Select2.Queries;
using WebTournament.Application.Tournament.GetTournament;
using WebTournament.Presentation.MVC.ViewModels;

namespace WebTournament.Presentation.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FighterController : BaseController
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
            return View(new FighterViewModel() {TournamentId = tournamentId});
        }

        [HttpGet("[controller]/{id}/[action]")]
        public async Task<IActionResult> EditIndex(Guid id)
        {
            var response = await _mediator.Send(new GetFighterQuery(id));
            return View(_mapper.Map<FighterViewModel>(response));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetFighterListQuery query, Guid tournamentId)
        {
            query.TournamentId = tournamentId;
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(FighterViewModel fighterViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
            var command = _mapper.Map<CreateFighterCommand>(fighterViewModel);
            await _mediator.Send(command);
            
            return CreatedAtAction(nameof(EditIndex), new { id = fighterViewModel.Id }, fighterViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(FighterViewModel fighterViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _mediator.Send(_mapper.Map<UpdateFighterCommand>(fighterViewModel));
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
        
        [HttpPost("[controller]/[action]/{bracketId}")]
        public async Task<IActionResult> Select2Fighters([FromForm]Select2FightersQuery request, [FromRoute] Guid bracketId)
        {
            request.Id = bracketId;
            return Ok(await _mediator.Send(request));
        }
    }
}

