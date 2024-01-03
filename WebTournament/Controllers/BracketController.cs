using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebTournament.Application.Bracket.DistributePlayers;
using WebTournament.Application.Bracket.GenerateBracket;
using WebTournament.Application.Bracket.GetBracket;
using WebTournament.Application.Bracket.GetBracketList;
using WebTournament.Application.Bracket.RemoveAllBrackets;
using WebTournament.Application.Bracket.RemoveBracket;
using WebTournament.Application.Bracket.SaveBracketState;
using WebTournament.Application.DTO;
using WebTournament.Application.Select2.Queries;

namespace WebTournament.Presentation.MVC.Controllers;

public class BracketController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public BracketController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }


    [HttpGet("[controller]/[action]/{tournamentId}")]
    public IActionResult TournamentBrackets(Guid tournamentId)
    {
        return View(tournamentId);
    }
    
    [HttpPost]
    public async Task<IActionResult> List([FromBody] GetBracketListQuery query, Guid tournamentId)
    {
        query.TournamentId = tournamentId;
        return Json(await _mediator.Send(query));
    }
    
    public IActionResult AddIndex(Guid tournamentId)
    {
        return View(new BracketDto()
        {
            TournamentId = tournamentId
        });
    }
    
    [HttpDelete("[controller]/{id}")]
    public async Task<IActionResult> DeleteModel(Guid id)
    {
        await _mediator.Send(new RemoveBracketCommand(id));
        return NoContent();
    }
    
    [HttpDelete("[controller]/[action]/{tournamentId}")]
    public async Task<IActionResult> DeleteAllBrackets(Guid tournamentId)
    {
        await _mediator.Send(new RemoveAllBracketsCommand(tournamentId));
        return NoContent();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddModel(BracketDto bracketDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
        await _mediator.Send(_mapper.Map<GenerateBracketCommand>(bracketDto));
        return CreatedAtAction(nameof(List), new { id = bracketDto.Id }, bracketDto);
    }
    
    
    [HttpPost("[controller]/[action]/{tournamentId}")]
    public async Task<IActionResult> DrawFighters(Guid tournamentId)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
        await _mediator.Send(new DistributePlayersCommand(tournamentId));
        return Ok();
    }

    [HttpGet("[controller]/[action]/{bracketId}")]
    public async Task<IActionResult> GetBracket(Guid bracketId)
    {
        var bracket = await _mediator.Send(new GetBracketQuery(bracketId));
        ViewData["Winners"] = JsonConvert.SerializeObject(bracket.Winners);
        return View("Bracket", bracket);
    }

    [HttpPost]
    public async Task<IActionResult> SaveState(BracketState bracketState)
    {
        await _mediator.Send(new SaveBracketStateCommand(bracketState.Id, bracketState));
        return Ok();
    }
    
    [HttpPost("[controller]/[action]/{bracketId}")]
    public async Task<IActionResult> Select2BracketFighters([FromForm]Select2FightersQuery request, Guid bracketId)
    {
        request.Id = bracketId;
        return Ok(await _mediator.Send(request));
    }
}