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

namespace TripBUK.Server.Controllers.Postgres
{
    [Route("odata/Postgres/Units")]
    public partial class UnitsController : ODataController
    {
        private TripBUK.Server.Data.PostgresContext context;

        public UnitsController(TripBUK.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<TripBUK.Server.Models.Postgres.Unit> GetUnits()
        {
            var items = this.context.Units.AsQueryable<TripBUK.Server.Models.Postgres.Unit>();
            this.OnUnitsRead(ref items);

            return items;
        }

        partial void OnUnitsRead(ref IQueryable<TripBUK.Server.Models.Postgres.Unit> items);

        partial void OnUnitGet(ref SingleResult<TripBUK.Server.Models.Postgres.Unit> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/Units(Id={Id})")]
        public SingleResult<TripBUK.Server.Models.Postgres.Unit> GetUnit(int key)
        {
            var items = this.context.Units.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnUnitGet(ref result);

            return result;
        }
        partial void OnUnitDeleted(TripBUK.Server.Models.Postgres.Unit item);
        partial void OnAfterUnitDeleted(TripBUK.Server.Models.Postgres.Unit item);

        [HttpDelete("/odata/Postgres/Units(Id={Id})")]
        public IActionResult DeleteUnit(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Units
                    .Where(i => i.Id == key)
                    .Include(i => i.UnitConversions)
                    .Include(i => i.UnitConversions1)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<TripBUK.Server.Models.Postgres.Unit>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnUnitDeleted(item);
                this.context.Units.Remove(item);
                this.context.SaveChanges();
                this.OnAfterUnitDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUnitUpdated(TripBUK.Server.Models.Postgres.Unit item);
        partial void OnAfterUnitUpdated(TripBUK.Server.Models.Postgres.Unit item);

        [HttpPut("/odata/Postgres/Units(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutUnit(int key, [FromBody]TripBUK.Server.Models.Postgres.Unit item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Units
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<TripBUK.Server.Models.Postgres.Unit>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnUnitUpdated(item);
                this.context.Units.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Units.Where(i => i.Id == key);
                
                this.OnAfterUnitUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/Units(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchUnit(int key, [FromBody]Delta<TripBUK.Server.Models.Postgres.Unit> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Units
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<TripBUK.Server.Models.Postgres.Unit>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnUnitUpdated(item);
                this.context.Units.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Units.Where(i => i.Id == key);
                
                this.OnAfterUnitUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUnitCreated(TripBUK.Server.Models.Postgres.Unit item);
        partial void OnAfterUnitCreated(TripBUK.Server.Models.Postgres.Unit item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] TripBUK.Server.Models.Postgres.Unit item)
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

                this.OnUnitCreated(item);
                this.context.Units.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Units.Where(i => i.Id == item.Id);

                

                this.OnAfterUnitCreated(item);

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
