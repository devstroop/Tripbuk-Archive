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
    }
}
