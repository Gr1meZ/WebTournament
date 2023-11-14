using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTournament.Business.Abstract
{
    public interface IExcelParserService
    {
        Task GenerateFromExcelAsync(IFormFile excelFile, CancellationToken cancellationToken);
    }
}
