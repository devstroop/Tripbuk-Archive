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
    [Route("odata/Postgres/Places")]
    public partial class PlacesController : ODataController
    {
        private Tripbuk.Server.Data.PostgresContext context;

        public PlacesController(Tripbuk.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Tripbuk.Server.Models.Postgres.Place> GetPlaces()
        {
            var items = this.context.Places.AsQueryable<Tripbuk.Server.Models.Postgres.Place>();
            this.OnPlacesRead(ref items);

            return items;
        }

        partial void OnPlacesRead(ref IQueryable<Tripbuk.Server.Models.Postgres.Place> items);

        partial void OnPlaceGet(ref SingleResult<Tripbuk.Server.Models.Postgres.Place> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/Places(Id={Id})")]
        public SingleResult<Tripbuk.Server.Models.Postgres.Place> GetPlace(Guid key)
        {
            var items = this.context.Places.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnPlaceGet(ref result);

            return result;
        }
        partial void OnPlaceDeleted(Tripbuk.Server.Models.Postgres.Place item);
        partial void OnAfterPlaceDeleted(Tripbuk.Server.Models.Postgres.Place item);

        [HttpDelete("/odata/Postgres/Places(Id={Id})")]
        public IActionResult DeletePlace(Guid key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Places
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnPlaceDeleted(item);
                this.context.Places.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPlaceDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPlaceUpdated(Tripbuk.Server.Models.Postgres.Place item);
        partial void OnAfterPlaceUpdated(Tripbuk.Server.Models.Postgres.Place item);

        [HttpPut("/odata/Postgres/Places(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPlace(Guid key, [FromBody]Tripbuk.Server.Models.Postgres.Place item)
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
                this.OnPlaceUpdated(item);
                this.context.Places.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Places.Where(i => i.Id == key);
                
                this.OnAfterPlaceUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/Places(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPlace(Guid key, [FromBody]Delta<Tripbuk.Server.Models.Postgres.Place> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Places.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnPlaceUpdated(item);
                this.context.Places.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Places.Where(i => i.Id == key);
                
                this.OnAfterPlaceUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPlaceCreated(Tripbuk.Server.Models.Postgres.Place item);
        partial void OnAfterPlaceCreated(Tripbuk.Server.Models.Postgres.Place item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Tripbuk.Server.Models.Postgres.Place item)
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

                this.OnPlaceCreated(item);
                this.context.Places.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Places.Where(i => i.Id == item.Id);

                

                this.OnAfterPlaceCreated(item);

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
