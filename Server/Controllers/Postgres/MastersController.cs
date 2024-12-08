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
    [Route("odata/Postgres/Masters")]
    public partial class MastersController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public MastersController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.Master> GetMasters()
        {
            var items = this.context.Masters.AsQueryable<ERP.Server.Models.Postgres.Master>();
            this.OnMastersRead(ref items);

            return items;
        }

        partial void OnMastersRead(ref IQueryable<ERP.Server.Models.Postgres.Master> items);

        partial void OnMasterGet(ref SingleResult<ERP.Server.Models.Postgres.Master> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/Masters(Id={Id})")]
        public SingleResult<ERP.Server.Models.Postgres.Master> GetMaster(int key)
        {
            var items = this.context.Masters.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnMasterGet(ref result);

            return result;
        }
        partial void OnMasterDeleted(ERP.Server.Models.Postgres.Master item);
        partial void OnAfterMasterDeleted(ERP.Server.Models.Postgres.Master item);

        [HttpDelete("/odata/Postgres/Masters(Id={Id})")]
        public IActionResult DeleteMaster(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Masters
                    .Where(i => i.Id == key)
                    .Include(i => i.StdNarrationMasters)
                    .Include(i => i.ItemGroupMasters)
                    .Include(i => i.ItemMasters)
                    .Include(i => i.AccountGroupMasters)
                    .Include(i => i.AccountMasters)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.Master>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMasterDeleted(item);
                this.context.Masters.Remove(item);
                this.context.SaveChanges();
                this.OnAfterMasterDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMasterUpdated(ERP.Server.Models.Postgres.Master item);
        partial void OnAfterMasterUpdated(ERP.Server.Models.Postgres.Master item);

        [HttpPut("/odata/Postgres/Masters(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutMaster(int key, [FromBody]ERP.Server.Models.Postgres.Master item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Masters
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.Master>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnMasterUpdated(item);
                this.context.Masters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Masters.Where(i => i.Id == key);
                
                this.OnAfterMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/Masters(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchMaster(int key, [FromBody]Delta<ERP.Server.Models.Postgres.Master> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Masters
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.Master>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnMasterUpdated(item);
                this.context.Masters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Masters.Where(i => i.Id == key);
                
                this.OnAfterMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnMasterCreated(ERP.Server.Models.Postgres.Master item);
        partial void OnAfterMasterCreated(ERP.Server.Models.Postgres.Master item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.Master item)
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

                this.OnMasterCreated(item);
                this.context.Masters.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Masters.Where(i => i.Id == item.Id);

                

                this.OnAfterMasterCreated(item);

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
