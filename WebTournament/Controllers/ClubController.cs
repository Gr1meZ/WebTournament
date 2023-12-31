﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Application.Club.CreateClub;
using WebTournament.Application.Club.GetClub;
using WebTournament.Application.Club.GetClubList;
using WebTournament.Application.Club.RemoveClub;
using WebTournament.Application.Club.UpdateClub;
using WebTournament.Application.Select2.Queries;
using WebTournament.Presentation.MVC.ViewModels;

namespace WebTournament.Presentation.MVC.Controllers
{
    public class ClubController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ClubController(IMediator mediator, IMapper mapper)
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
            var response = await _mediator.Send(new GetClubQuery(id));
            return View(_mapper.Map<ClubViewModel>(response));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetClubListQuery query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(ClubViewModel clubViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage).ToList());
           
            var command = _mapper.Map<CreateClubCommand>(clubViewModel);
            await _mediator.Send(command);
            
            return CreatedAtAction(nameof(EditIndex), new { id = clubViewModel.Id }, clubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(ClubViewModel clubViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _mediator.Send(_mapper.Map<UpdateClubCommand>(clubViewModel));
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _mediator.Send(new RemoveClubCommand(id));
            return NoContent();
        }

        public async Task<IActionResult> Select2Clubs([FromForm] Select2ClubsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}