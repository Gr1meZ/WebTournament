using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Application.DTO;
using WebTournament.Application.Select2.Queries;
using WebTournament.Application.Trainer.CreateTrainer;
using WebTournament.Application.Trainer.GetTrainer;
using WebTournament.Application.Trainer.GetTrainerList;
using WebTournament.Application.Trainer.RemoveTrainer;
using WebTournament.Application.Trainer.UpdateTrainer;

namespace WebTournament.Presentation.MVC.Controllers
{
    public class TrainerController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TrainerController(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
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
            return View(await _mediator.Send(new GetTrainerQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetTrainerListQuery query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(TrainerDto trainerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage).ToList());
           
            var command = _mapper.Map<CreateTrainerCommand>(trainerDto);
            await _mediator.Send(command);
            
            return CreatedAtAction(nameof(EditIndex), new { id = trainerDto.Id }, trainerDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(TrainerDto trainerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _mediator.Send(_mapper.Map<UpdateTrainerCommand>(trainerDto));
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _mediator.Send(new RemoveTrainerCommand(id));
            return NoContent();
        }

        public async Task<IActionResult> Select2Trainers([FromForm] Select2TrainersQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
