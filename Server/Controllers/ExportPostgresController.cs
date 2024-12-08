using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using ERP.Server.Data;

namespace ERP.Server.Controllers
{
    public partial class ExportPostgresController : ExportController
    {
        private readonly PostgresContext _context;
        private readonly PostgresService _service;

        public ExportPostgresController(PostgresContext context, PostgresService service)
        {
            this._service = service;
            this._context = context;
        }

        [HttpGet("/export/Postgres/accountgroups/csv")]
        [HttpGet("/export/Postgres/accountgroups/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountGroupsToCsv(string fileName = null)
        {
            return ToCsv(ApplyQuery(await _service.GetAccountGroups(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accountgroups/excel")]
        [HttpGet("/export/Postgres/accountgroups/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountGroupsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _service.GetAccountGroups(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accounts/csv")]
        [HttpGet("/export/Postgres/accounts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountsToCsv(string fileName = null)
        {
            return ToCsv(ApplyQuery(await _service.GetAccounts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accounts/excel")]
        [HttpGet("/export/Postgres/accounts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _service.GetAccounts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/itemgroups/csv")]
        [HttpGet("/export/Postgres/itemgroups/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemGroupsToCsv(string fileName = null)
        {
            return ToCsv(ApplyQuery(await _service.GetItemGroups(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/itemgroups/excel")]
        [HttpGet("/export/Postgres/itemgroups/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemGroupsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _service.GetItemGroups(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/items/csv")]
        [HttpGet("/export/Postgres/items/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemsToCsv(string fileName = null)
        {
            return ToCsv(ApplyQuery(await _service.GetItems(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/items/excel")]
        [HttpGet("/export/Postgres/items/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _service.GetItems(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/standardnarrations/csv")]
        [HttpGet("/export/Postgres/standardnarrations/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStandardNarrationsToCsv(string fileName = null)
        {
            return ToCsv(ApplyQuery(await _service.GetStandardNarrations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/standardnarrations/excel")]
        [HttpGet("/export/Postgres/standardnarrations/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStandardNarrationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _service.GetStandardNarrations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/units/csv")]
        [HttpGet("/export/Postgres/units/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUnitsToCsv(string fileName = null)
        {
            return ToCsv(ApplyQuery(await _service.GetUnits(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/units/excel")]
        [HttpGet("/export/Postgres/units/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUnitsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await _service.GetUnits(), Request.Query, false), fileName);
        }
    }
}
