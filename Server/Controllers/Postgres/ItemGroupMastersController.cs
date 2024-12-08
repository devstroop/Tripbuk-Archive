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
    [Route("odata/Postgres/ItemGroupMasters")]
    public partial class ItemGroupMastersController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public ItemGroupMastersController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.ItemGroupMaster> GetItemGroupMasters()
        {
            var items = this.context.ItemGroupMasters.AsQueryable<ERP.Server.Models.Postgres.ItemGroupMaster>();
            this.OnItemGroupMastersRead(ref items);

            return items;
        }

        partial void OnItemGroupMastersRead(ref IQueryable<ERP.Server.Models.Postgres.ItemGroupMaster> items);

        partial void OnItemGroupMasterGet(ref SingleResult<ERP.Server.Models.Postgres.ItemGroupMaster> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/ItemGroupMasters(MasterId={MasterId})")]
        public SingleResult<ERP.Server.Models.Postgres.ItemGroupMaster> GetItemGroupMaster(int key)
        {
            var items = this.context.ItemGroupMasters.Where(i => i.MasterId == key);
            var result = SingleResult.Create(items);

            OnItemGroupMasterGet(ref result);

            return result;
        }
        partial void OnItemGroupMasterDeleted(ERP.Server.Models.Postgres.ItemGroupMaster item);
        partial void OnAfterItemGroupMasterDeleted(ERP.Server.Models.Postgres.ItemGroupMaster item);

        [HttpDelete("/odata/Postgres/ItemGroupMasters(MasterId={MasterId})")]
        public IActionResult DeleteItemGroupMaster(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.ItemGroupMasters
                    .Where(i => i.MasterId == key)
                    .Include(i => i.ItemGroupMasters1)
                    .Include(i => i.ItemMasters)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.ItemGroupMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnItemGroupMasterDeleted(item);
                this.context.ItemGroupMasters.Remove(item);
                this.context.SaveChanges();
                this.OnAfterItemGroupMasterDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnItemGroupMasterUpdated(ERP.Server.Models.Postgres.ItemGroupMaster item);
        partial void OnAfterItemGroupMasterUpdated(ERP.Server.Models.Postgres.ItemGroupMaster item);

        [HttpPut("/odata/Postgres/ItemGroupMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutItemGroupMaster(int key, [FromBody]ERP.Server.Models.Postgres.ItemGroupMaster item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ItemGroupMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.ItemGroupMaster>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnItemGroupMasterUpdated(item);
                this.context.ItemGroupMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ItemGroupMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Master,ItemGroupMaster1");
                this.OnAfterItemGroupMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/ItemGroupMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchItemGroupMaster(int key, [FromBody]Delta<ERP.Server.Models.Postgres.ItemGroupMaster> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ItemGroupMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.ItemGroupMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnItemGroupMasterUpdated(item);
                this.context.ItemGroupMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ItemGroupMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Master,ItemGroupMaster1");
                this.OnAfterItemGroupMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnItemGroupMasterCreated(ERP.Server.Models.Postgres.ItemGroupMaster item);
        partial void OnAfterItemGroupMasterCreated(ERP.Server.Models.Postgres.ItemGroupMaster item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.ItemGroupMaster item)
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

                this.OnItemGroupMasterCreated(item);
                this.context.ItemGroupMasters.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ItemGroupMasters.Where(i => i.MasterId == item.MasterId);

                Request.QueryString = Request.QueryString.Add("$expand", "Master,ItemGroupMaster1");

                this.OnAfterItemGroupMasterCreated(item);

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
