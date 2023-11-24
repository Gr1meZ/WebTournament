﻿using Microsoft.AspNetCore.Http;

namespace WebTournament.Business.Abstract
{
    public interface IExcelParserService
    {
        Task GenerateFromExcelAsync(IFormFile excelFile, Guid tournamentId, CancellationToken cancellationToken);
    }
}
