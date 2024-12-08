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

namespace ERP.Server.Controllers.Postgres
{
    [Route("odata/Postgres/Units")]
    public partial class UnitsController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public UnitsController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.Unit> GetUnits()
        {
            var items = this.context.Units.AsQueryable<ERP.Server.Models.Postgres.Unit>();
            this.OnUnitsRead(ref items);

            return items;
        }

        partial void OnUnitsRead(ref IQueryable<ERP.Server.Models.Postgres.Unit> items);

        partial void OnUnitGet(ref SingleResult<ERP.Server.Models.Postgres.Unit> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/Units(Id={Id})")]
        public SingleResult<ERP.Server.Models.Postgres.Unit> GetUnit(int key)
        {
            var items = this.context.Units.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnUnitGet(ref result);

            return result;
        }
        partial void OnUnitDeleted(ERP.Server.Models.Postgres.Unit item);
        partial void OnAfterUnitDeleted(ERP.Server.Models.Postgres.Unit item);

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
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.Unit>(Request, items);

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

        partial void OnUnitUpdated(ERP.Server.Models.Postgres.Unit item);
        partial void OnAfterUnitUpdated(ERP.Server.Models.Postgres.Unit item);

        [HttpPut("/odata/Postgres/Units(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutUnit(int key, [FromBody]ERP.Server.Models.Postgres.Unit item)
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

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.Unit>(Request, items);

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
        public IActionResult PatchUnit(int key, [FromBody]Delta<ERP.Server.Models.Postgres.Unit> patch)
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

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.Unit>(Request, items);

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

        partial void OnUnitCreated(ERP.Server.Models.Postgres.Unit item);
        partial void OnAfterUnitCreated(ERP.Server.Models.Postgres.Unit item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.Unit item)
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
