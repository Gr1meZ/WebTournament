using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;

namespace WebTournament.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ExcelParserController : Controller
    {
        private readonly IExcelParserService _excelParserService;
        public ExcelParserController(IExcelParserService excelParserService)
        {
            _excelParserService = excelParserService;

        }

        [HttpPost]
        public async Task<IActionResult> Read([FromQuery]Guid tournamentId, CancellationToken cancellationToken)
        {
            var formFile = Request.Form.Files[0];

            await _excelParserService.GenerateFromExcelAsync(formFile, tournamentId, cancellationToken);
            return Ok();
        }
    }
}
