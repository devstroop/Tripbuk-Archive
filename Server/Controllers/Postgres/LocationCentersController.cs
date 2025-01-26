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
    [Route("odata/Postgres/LocationCenters")]
    public partial class LocationCentersController : ODataController
    {
        private Tripbuk.Server.Data.PostgresContext context;

        public LocationCentersController(Tripbuk.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Tripbuk.Server.Models.Postgres.LocationCenter> GetLocationCenters()
        {
            var items = this.context.LocationCenters.AsQueryable<Tripbuk.Server.Models.Postgres.LocationCenter>();
            this.OnLocationCentersRead(ref items);

            return items;
        }

        partial void OnLocationCentersRead(ref IQueryable<Tripbuk.Server.Models.Postgres.LocationCenter> items);

        partial void OnLocationCenterGet(ref SingleResult<Tripbuk.Server.Models.Postgres.LocationCenter> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/LocationCenters(Id={Id})")]
        public SingleResult<Tripbuk.Server.Models.Postgres.LocationCenter> GetLocationCenter(int key)
        {
            var items = this.context.LocationCenters.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnLocationCenterGet(ref result);

            return result;
        }
        partial void OnLocationCenterDeleted(Tripbuk.Server.Models.Postgres.LocationCenter item);
        partial void OnAfterLocationCenterDeleted(Tripbuk.Server.Models.Postgres.LocationCenter item);

        [HttpDelete("/odata/Postgres/LocationCenters(Id={Id})")]
        public IActionResult DeleteLocationCenter(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.LocationCenters
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnLocationCenterDeleted(item);
                this.context.LocationCenters.Remove(item);
                this.context.SaveChanges();
                this.OnAfterLocationCenterDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLocationCenterUpdated(Tripbuk.Server.Models.Postgres.LocationCenter item);
        partial void OnAfterLocationCenterUpdated(Tripbuk.Server.Models.Postgres.LocationCenter item);

        [HttpPut("/odata/Postgres/LocationCenters(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutLocationCenter(int key, [FromBody]Tripbuk.Server.Models.Postgres.LocationCenter item)
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
                this.OnLocationCenterUpdated(item);
                this.context.LocationCenters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LocationCenters.Where(i => i.Id == key);
                
                this.OnAfterLocationCenterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/LocationCenters(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchLocationCenter(int key, [FromBody]Delta<Tripbuk.Server.Models.Postgres.LocationCenter> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.LocationCenters.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnLocationCenterUpdated(item);
                this.context.LocationCenters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LocationCenters.Where(i => i.Id == key);
                
                this.OnAfterLocationCenterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnLocationCenterCreated(Tripbuk.Server.Models.Postgres.LocationCenter item);
        partial void OnAfterLocationCenterCreated(Tripbuk.Server.Models.Postgres.LocationCenter item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Tripbuk.Server.Models.Postgres.LocationCenter item)
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

                this.OnLocationCenterCreated(item);
                this.context.LocationCenters.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.LocationCenters.Where(i => i.Id == item.Id);

                

                this.OnAfterLocationCenterCreated(item);

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
