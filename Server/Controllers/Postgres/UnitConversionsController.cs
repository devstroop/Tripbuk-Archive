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
    [Route("odata/Postgres/UnitConversions")]
    public partial class UnitConversionsController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public UnitConversionsController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.UnitConversion> GetUnitConversions()
        {
            var items = this.context.UnitConversions.AsQueryable<ERP.Server.Models.Postgres.UnitConversion>();
            this.OnUnitConversionsRead(ref items);

            return items;
        }

        partial void OnUnitConversionsRead(ref IQueryable<ERP.Server.Models.Postgres.UnitConversion> items);

        partial void OnUnitConversionGet(ref SingleResult<ERP.Server.Models.Postgres.UnitConversion> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/UnitConversions(Id={Id})")]
        public SingleResult<ERP.Server.Models.Postgres.UnitConversion> GetUnitConversion(int key)
        {
            var items = this.context.UnitConversions.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnUnitConversionGet(ref result);

            return result;
        }
        partial void OnUnitConversionDeleted(ERP.Server.Models.Postgres.UnitConversion item);
        partial void OnAfterUnitConversionDeleted(ERP.Server.Models.Postgres.UnitConversion item);

        [HttpDelete("/odata/Postgres/UnitConversions(Id={Id})")]
        public IActionResult DeleteUnitConversion(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.UnitConversions
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.UnitConversion>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnUnitConversionDeleted(item);
                this.context.UnitConversions.Remove(item);
                this.context.SaveChanges();
                this.OnAfterUnitConversionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUnitConversionUpdated(ERP.Server.Models.Postgres.UnitConversion item);
        partial void OnAfterUnitConversionUpdated(ERP.Server.Models.Postgres.UnitConversion item);

        [HttpPut("/odata/Postgres/UnitConversions(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutUnitConversion(int key, [FromBody]ERP.Server.Models.Postgres.UnitConversion item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.UnitConversions
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.UnitConversion>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnUnitConversionUpdated(item);
                this.context.UnitConversions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.UnitConversions.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Unit,Unit1,AspNetTenant");
                this.OnAfterUnitConversionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/UnitConversions(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchUnitConversion(int key, [FromBody]Delta<ERP.Server.Models.Postgres.UnitConversion> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.UnitConversions
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.UnitConversion>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnUnitConversionUpdated(item);
                this.context.UnitConversions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.UnitConversions.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Unit,Unit1,AspNetTenant");
                this.OnAfterUnitConversionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnUnitConversionCreated(ERP.Server.Models.Postgres.UnitConversion item);
        partial void OnAfterUnitConversionCreated(ERP.Server.Models.Postgres.UnitConversion item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.UnitConversion item)
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

                this.OnUnitConversionCreated(item);
                this.context.UnitConversions.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.UnitConversions.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "Unit,Unit1,AspNetTenant");

                this.OnAfterUnitConversionCreated(item);

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
