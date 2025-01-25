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
    [Route("odata/Postgres/Destinations")]
    public partial class DestinationsController : ODataController
    {
        private Tripbuk.Server.Data.PostgresContext context;

        public DestinationsController(Tripbuk.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Tripbuk.Server.Models.Postgres.Destination> GetDestinations()
        {
            var items = this.context.Destinations.AsQueryable<Tripbuk.Server.Models.Postgres.Destination>();
            this.OnDestinationsRead(ref items);

            return items;
        }

        partial void OnDestinationsRead(ref IQueryable<Tripbuk.Server.Models.Postgres.Destination> items);

        partial void OnDestinationGet(ref SingleResult<Tripbuk.Server.Models.Postgres.Destination> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/Destinations(Id={Id})")]
        public SingleResult<Tripbuk.Server.Models.Postgres.Destination> GetDestination(int key)
        {
            var items = this.context.Destinations.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnDestinationGet(ref result);

            return result;
        }
        partial void OnDestinationDeleted(Tripbuk.Server.Models.Postgres.Destination item);
        partial void OnAfterDestinationDeleted(Tripbuk.Server.Models.Postgres.Destination item);

        [HttpDelete("/odata/Postgres/Destinations(Id={Id})")]
        public IActionResult DeleteDestination(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Destinations
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnDestinationDeleted(item);
                this.context.Destinations.Remove(item);
                this.context.SaveChanges();
                this.OnAfterDestinationDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDestinationUpdated(Tripbuk.Server.Models.Postgres.Destination item);
        partial void OnAfterDestinationUpdated(Tripbuk.Server.Models.Postgres.Destination item);

        [HttpPut("/odata/Postgres/Destinations(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutDestination(int key, [FromBody]Tripbuk.Server.Models.Postgres.Destination item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Id != key))
                {
                    return BadRequest();
                }
                this.OnDestinationUpdated(item);
                this.context.Destinations.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Destinations.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Destination1");
                this.OnAfterDestinationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/Destinations(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchDestination(int key, [FromBody]Delta<Tripbuk.Server.Models.Postgres.Destination> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Destinations.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnDestinationUpdated(item);
                this.context.Destinations.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Destinations.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Destination1");
                this.OnAfterDestinationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDestinationCreated(Tripbuk.Server.Models.Postgres.Destination item);
        partial void OnAfterDestinationCreated(Tripbuk.Server.Models.Postgres.Destination item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Tripbuk.Server.Models.Postgres.Destination item)
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

                this.OnDestinationCreated(item);
                this.context.Destinations.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Destinations.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "Destination1");

                this.OnAfterDestinationCreated(item);

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
