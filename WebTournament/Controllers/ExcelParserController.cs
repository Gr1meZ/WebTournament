﻿using Microsoft.AspNetCore.Mvc;
using WebTournament.Business.Abstract;

namespace WebTournament.WebApp.Controllers
{
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
            Path.GetTempFileName();

            if (formFile is not { Length: > 0 })
                return BadRequest("Файл пустой");

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Не поддерживаемый формат файла");

            await _excelParserService.GenerateFromExcelAsync(formFile, tournamentId, cancellationToken);
            return Ok();
        }
    }
}