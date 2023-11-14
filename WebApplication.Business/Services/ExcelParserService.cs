using Infrastructure.DataAccess.Abstract;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebTournament.Business.Abstract;

namespace WebTournament.Business.Services
{
    public class ExcelParserService : IExcelParserService
    {
        private readonly IApplicationDbContext _appDbContext;
        public ExcelParserService(IApplicationDbContext appDbContext) {
            this._appDbContext = appDbContext;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        }

        public async Task GenerateFromExcelAsync(IFormFile excelFile, CancellationToken cancellationToken)
        {

            using var stream = new MemoryStream();

            await excelFile.CopyToAsync(stream, cancellationToken);

            using var package = new ExcelPackage(stream);

            var worksheet = package.Workbook.Worksheets[0]; // Индекс листа
            var rowCount = worksheet.Dimension.Rows;
            var colCount = worksheet.Dimension.Columns;

            for (int row = 1; row <= rowCount; row++)
            {
                for (int col = 1; col <= colCount; col++)
                {
                    Console.Write("{0}\t", worksheet.Cells[row, col].Value);
                }
            }
        }
    }

    }

