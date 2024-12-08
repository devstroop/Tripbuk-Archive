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
    [Route("odata/Postgres/StandardNarrations")]
    public partial class StandardNarrationsController : ODataController
    {
        private ERP.Server.Data.PostgresContext _context;

        public StandardNarrationsController(ERP.Server.Data.PostgresContext context)
        {
            this._context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.StandardNarration> GetStandardNarrations()
        {
            var items = this._context.StandardNarrations.AsQueryable<ERP.Server.Models.Postgres.StandardNarration>();
            this.OnStandardNarrationsRead(ref items);

            return items;
        }

        partial void OnStandardNarrationsRead(ref IQueryable<ERP.Server.Models.Postgres.StandardNarration> items);

        partial void OnStandardNarrationGet(ref SingleResult<ERP.Server.Models.Postgres.StandardNarration> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/StandardNarrations(Id={Id})")]
        public SingleResult<ERP.Server.Models.Postgres.StandardNarration> GetStandardNarration(int key)
        {
            var items = this._context.StandardNarrations.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnStandardNarrationGet(ref result);

            return result;
        }
        partial void OnStandardNarrationDeleted(ERP.Server.Models.Postgres.StandardNarration item);
        partial void OnAfterStandardNarrationDeleted(ERP.Server.Models.Postgres.StandardNarration item);

        [HttpDelete("/odata/Postgres/StandardNarrations(Id={Id})")]
        public IActionResult DeleteStandardNarration(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this._context.StandardNarrations
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.StandardNarration>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStandardNarrationDeleted(item);
                this._context.StandardNarrations.Remove(item);
                this._context.SaveChanges();
                this.OnAfterStandardNarrationDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStandardNarrationUpdated(ERP.Server.Models.Postgres.StandardNarration item);
        partial void OnAfterStandardNarrationUpdated(ERP.Server.Models.Postgres.StandardNarration item);

        [HttpPut("/odata/Postgres/StandardNarrations(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStandardNarration(int key, [FromBody]ERP.Server.Models.Postgres.StandardNarration item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this._context.StandardNarrations
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.StandardNarration>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStandardNarrationUpdated(item);
                this._context.StandardNarrations.Update(item);
                this._context.SaveChanges();

                var itemToReturn = this._context.StandardNarrations.Where(i => i.Id == key);
                
                this.OnAfterStandardNarrationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/StandardNarrations(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStandardNarration(int key, [FromBody]Delta<ERP.Server.Models.Postgres.StandardNarration> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this._context.StandardNarrations
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.StandardNarration>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStandardNarrationUpdated(item);
                this._context.StandardNarrations.Update(item);
                this._context.SaveChanges();

                var itemToReturn = this._context.StandardNarrations.Where(i => i.Id == key);
                
                this.OnAfterStandardNarrationUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStandardNarrationCreated(ERP.Server.Models.Postgres.StandardNarration item);
        partial void OnAfterStandardNarrationCreated(ERP.Server.Models.Postgres.StandardNarration item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.StandardNarration item)
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

                this.OnStandardNarrationCreated(item);
                this._context.StandardNarrations.Add(item);
                this._context.SaveChanges();

                var itemToReturn = this._context.StandardNarrations.Where(i => i.Id == item.Id);

                

                this.OnAfterStandardNarrationCreated(item);

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
