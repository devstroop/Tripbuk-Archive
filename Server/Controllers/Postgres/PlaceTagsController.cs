using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Tripbuk.Server.Controllers.Postgres
{
    [Route("odata/Postgres/PlaceTags")]
    public partial class PlaceTagsController : ODataController
    {
        private Tripbuk.Server.Data.PostgresContext context;

        public PlaceTagsController(Tripbuk.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Tripbuk.Server.Models.Postgres.PlaceTag> GetPlaceTags()
        {
            var items = this.context.PlaceTags.AsQueryable<Tripbuk.Server.Models.Postgres.PlaceTag>();
            this.OnPlaceTagsRead(ref items);

            return items;
        }

        partial void OnPlaceTagsRead(ref IQueryable<Tripbuk.Server.Models.Postgres.PlaceTag> items);
    }
}
