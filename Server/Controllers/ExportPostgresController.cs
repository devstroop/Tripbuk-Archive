using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using TripBUK.Server.Data;

namespace TripBUK.Server.Controllers
{
    public partial class ExportPostgresController : ExportController
    {
        private readonly PostgresContext context;
        private readonly PostgresService service;

        public ExportPostgresController(PostgresContext context, PostgresService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/Postgres/accountgroups/csv")]
        [HttpGet("/export/Postgres/accountgroups/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountGroupsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAccountGroups(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accountgroups/excel")]
        [HttpGet("/export/Postgres/accountgroups/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountGroupsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAccountGroups(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accounts/csv")]
        [HttpGet("/export/Postgres/accounts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAccounts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accounts/excel")]
        [HttpGet("/export/Postgres/accounts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAccounts(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/itemgroups/csv")]
        [HttpGet("/export/Postgres/itemgroups/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemGroupsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetItemGroups(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/itemgroups/excel")]
        [HttpGet("/export/Postgres/itemgroups/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemGroupsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetItemGroups(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/items/csv")]
        [HttpGet("/export/Postgres/items/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetItems(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/items/excel")]
        [HttpGet("/export/Postgres/items/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetItems(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/standardnarrations/csv")]
        [HttpGet("/export/Postgres/standardnarrations/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStandardNarrationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStandardNarrations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/standardnarrations/excel")]
        [HttpGet("/export/Postgres/standardnarrations/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStandardNarrationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStandardNarrations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/unitconversions/csv")]
        [HttpGet("/export/Postgres/unitconversions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUnitConversionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetUnitConversions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/unitconversions/excel")]
        [HttpGet("/export/Postgres/unitconversions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUnitConversionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetUnitConversions(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/units/csv")]
        [HttpGet("/export/Postgres/units/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUnitsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetUnits(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/units/excel")]
        [HttpGet("/export/Postgres/units/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportUnitsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetUnits(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/smtpconfigs/csv")]
        [HttpGet("/export/Postgres/smtpconfigs/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSmtpConfigsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetSmtpConfigs(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/smtpconfigs/excel")]
        [HttpGet("/export/Postgres/smtpconfigs/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportSmtpConfigsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetSmtpConfigs(), Request.Query, false), fileName);
        }
    }
}
