using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Application.Common;
using WebTournament.Application.DTO;
using WebTournament.Application.Select2.Queries;
using WebTournament.Application.WeightCategorie.CreateWeightCategorie;
using WebTournament.Application.WeightCategorie.GetWeightCategorie;
using WebTournament.Application.WeightCategorie.GetWeightCategorieList;
using WebTournament.Application.WeightCategorie.RemoveWeightCategorie;
using WebTournament.Application.WeightCategorie.UpdateWeightCategorie;

namespace WebTournament.Presentation.MVC.Controllers
{

    public class WeightCategorieController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public WeightCategorieController(IMediator mediator, IMapper mapper)
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
            return View(await _mediator.Send(new GetWeightCategorieQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> List([FromBody] GetWeightCategorieListQuery query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> AddModel(WeightCategorieDto weightCategorieDto)
        {
            if (!ModelState.IsValid || !weightCategorieDto.AgeGroupId.HasValue) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());
           
            var command = _mapper.Map<CreateWeightCategorieCommand>(weightCategorieDto);
            await _mediator.Send(command);
            
            return CreatedAtAction(nameof(EditIndex), new { id = weightCategorieDto.Id }, weightCategorieDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditModel(WeightCategorieDto weightCategorieDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList());

            await _mediator.Send(_mapper.Map<UpdateWeightCategorieCommand>(weightCategorieDto));
            return Ok();
        }

        [HttpDelete("[controller]/{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            await _mediator.Send(new RemoveWeightCategorieCommand(id));
            return NoContent();
        }

        public async Task<IActionResult> Select2WeightCategories([FromForm] Select2WeightCategorieQuery query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
