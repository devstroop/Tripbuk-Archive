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
        private readonly PostgresContext context;
        private readonly PostgresService service;

        public ExportPostgresController(PostgresContext context, PostgresService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/Postgres/masters/csv")]
        [HttpGet("/export/Postgres/masters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMastersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/masters/excel")]
        [HttpGet("/export/Postgres/masters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportMastersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/stdnarrationmasters/csv")]
        [HttpGet("/export/Postgres/stdnarrationmasters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStdNarrationMastersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStdNarrationMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/stdnarrationmasters/excel")]
        [HttpGet("/export/Postgres/stdnarrationmasters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStdNarrationMastersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStdNarrationMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/itemmasters/csv")]
        [HttpGet("/export/Postgres/itemmasters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemMastersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetItemMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/itemmasters/excel")]
        [HttpGet("/export/Postgres/itemmasters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemMastersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetItemMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/itemgroupmasters/csv")]
        [HttpGet("/export/Postgres/itemgroupmasters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemGroupMastersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetItemGroupMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/itemgroupmasters/excel")]
        [HttpGet("/export/Postgres/itemgroupmasters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportItemGroupMastersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetItemGroupMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accountmasters/csv")]
        [HttpGet("/export/Postgres/accountmasters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountMastersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAccountMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accountmasters/excel")]
        [HttpGet("/export/Postgres/accountmasters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountMastersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAccountMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accountgroupmasters/csv")]
        [HttpGet("/export/Postgres/accountgroupmasters/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountGroupMastersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAccountGroupMasters(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/accountgroupmasters/excel")]
        [HttpGet("/export/Postgres/accountgroupmasters/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAccountGroupMastersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAccountGroupMasters(), Request.Query, false), fileName);
        }
    }
}
