using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Tripbuk.Server.Data;

namespace Tripbuk.Server.Controllers
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

        [HttpGet("/export/Postgres/destinations/csv")]
        [HttpGet("/export/Postgres/destinations/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDestinationsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDestinations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/destinations/excel")]
        [HttpGet("/export/Postgres/destinations/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDestinationsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDestinations(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/parenttags/csv")]
        [HttpGet("/export/Postgres/parenttags/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportParentTagsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetParentTags(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/parenttags/excel")]
        [HttpGet("/export/Postgres/parenttags/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportParentTagsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetParentTags(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/places/csv")]
        [HttpGet("/export/Postgres/places/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlacesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPlaces(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/places/excel")]
        [HttpGet("/export/Postgres/places/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlacesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPlaces(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/placetags/csv")]
        [HttpGet("/export/Postgres/placetags/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlaceTagsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPlaceTags(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/placetags/excel")]
        [HttpGet("/export/Postgres/placetags/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlaceTagsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPlaceTags(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/tags/csv")]
        [HttpGet("/export/Postgres/tags/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTagsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTags(), Request.Query, false), fileName);
        }

        [HttpGet("/export/Postgres/tags/excel")]
        [HttpGet("/export/Postgres/tags/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTagsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTags(), Request.Query, false), fileName);
        }
    }
}
