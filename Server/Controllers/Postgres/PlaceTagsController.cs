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

        partial void OnPlaceTagGet(ref SingleResult<Tripbuk.Server.Models.Postgres.PlaceTag> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/PlaceTags(PlaceId={keyPlaceId},TagId={keyTagId})")]
        public SingleResult<Tripbuk.Server.Models.Postgres.PlaceTag> GetPlaceTag([FromODataUri] Guid keyPlaceId, [FromODataUri] int keyTagId)
        {
            var items = this.context.PlaceTags.Where(i => i.PlaceId == keyPlaceId && i.TagId == keyTagId);
            var result = SingleResult.Create(items);

            OnPlaceTagGet(ref result);

            return result;
        }
        partial void OnPlaceTagDeleted(Tripbuk.Server.Models.Postgres.PlaceTag item);
        partial void OnAfterPlaceTagDeleted(Tripbuk.Server.Models.Postgres.PlaceTag item);

        [HttpDelete("/odata/Postgres/PlaceTags(PlaceId={keyPlaceId},TagId={keyTagId})")]
        public IActionResult DeletePlaceTag([FromODataUri] Guid keyPlaceId, [FromODataUri] int keyTagId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.PlaceTags
                    .Where(i => i.PlaceId == keyPlaceId && i.TagId == keyTagId)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnPlaceTagDeleted(item);
                this.context.PlaceTags.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPlaceTagDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPlaceTagUpdated(Tripbuk.Server.Models.Postgres.PlaceTag item);
        partial void OnAfterPlaceTagUpdated(Tripbuk.Server.Models.Postgres.PlaceTag item);

        [HttpPut("/odata/Postgres/PlaceTags(PlaceId={keyPlaceId},TagId={keyTagId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPlaceTag([FromODataUri] Guid keyPlaceId, [FromODataUri] int keyTagId, [FromBody]Tripbuk.Server.Models.Postgres.PlaceTag item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.PlaceId != keyPlaceId && item.TagId != keyTagId))
                {
                    return BadRequest();
                }
                this.OnPlaceTagUpdated(item);
                this.context.PlaceTags.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.PlaceTags.Where(i => i.PlaceId == keyPlaceId && i.TagId == keyTagId);
                Request.QueryString = Request.QueryString.Add("$expand", "Place,Tag");
                this.OnAfterPlaceTagUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/PlaceTags(PlaceId={keyPlaceId},TagId={keyTagId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPlaceTag([FromODataUri] Guid keyPlaceId, [FromODataUri] int keyTagId, [FromBody]Delta<Tripbuk.Server.Models.Postgres.PlaceTag> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.PlaceTags.Where(i => i.PlaceId == keyPlaceId && i.TagId == keyTagId).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnPlaceTagUpdated(item);
                this.context.PlaceTags.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.PlaceTags.Where(i => i.PlaceId == keyPlaceId && i.TagId == keyTagId);
                Request.QueryString = Request.QueryString.Add("$expand", "Place,Tag");
                this.OnAfterPlaceTagUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPlaceTagCreated(Tripbuk.Server.Models.Postgres.PlaceTag item);
        partial void OnAfterPlaceTagCreated(Tripbuk.Server.Models.Postgres.PlaceTag item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Tripbuk.Server.Models.Postgres.PlaceTag item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnPlaceTagCreated(item);
                this.context.PlaceTags.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.PlaceTags.Where(i => i.PlaceId == item.PlaceId && i.TagId == item.TagId);

                Request.QueryString = Request.QueryString.Add("$expand", "Place,Tag");

                this.OnAfterPlaceTagCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
