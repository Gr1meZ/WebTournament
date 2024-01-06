using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Application.Select2.Queries;
using WebTournament.Application.Trainer.CreateTrainer;
using WebTournament.Application.Trainer.GetTrainer;
using WebTournament.Application.Trainer.GetTrainerList;
using WebTournament.Application.Trainer.RemoveTrainer;
using WebTournament.Application.Trainer.UpdateTrainer;
using WebTournament.Presentation.MVC.ViewModels;

namespace WebTournament.Presentation.MVC.Controllers
{
    public class TrainerController : BaseController
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
            var response = await _mediator.Send(new GetTrainerQuery(id));
            return View(_mapper.Map<TrainerViewModel>(response));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetTrainerListQuery query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(TrainerViewModel trainerViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage).ToList());
           
            var command = _mapper.Map<CreateTrainerCommand>(trainerViewModel);
            await _mediator.Send(command);
            
            return CreatedAtAction(nameof(EditIndex), new { id = trainerViewModel.Id }, trainerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(TrainerViewModel trainerViewModel)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _mediator.Send(_mapper.Map<UpdateTrainerCommand>(trainerViewModel));
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
