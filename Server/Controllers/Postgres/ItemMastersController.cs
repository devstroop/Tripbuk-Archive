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
    [Route("odata/Postgres/ItemMasters")]
    public partial class ItemMastersController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public ItemMastersController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.ItemMaster> GetItemMasters()
        {
            var items = this.context.ItemMasters.AsQueryable<ERP.Server.Models.Postgres.ItemMaster>();
            this.OnItemMastersRead(ref items);

            return items;
        }

        partial void OnItemMastersRead(ref IQueryable<ERP.Server.Models.Postgres.ItemMaster> items);

        partial void OnItemMasterGet(ref SingleResult<ERP.Server.Models.Postgres.ItemMaster> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/ItemMasters(MasterId={MasterId})")]
        public SingleResult<ERP.Server.Models.Postgres.ItemMaster> GetItemMaster(int key)
        {
            var items = this.context.ItemMasters.Where(i => i.MasterId == key);
            var result = SingleResult.Create(items);

            OnItemMasterGet(ref result);

            return result;
        }
        partial void OnItemMasterDeleted(ERP.Server.Models.Postgres.ItemMaster item);
        partial void OnAfterItemMasterDeleted(ERP.Server.Models.Postgres.ItemMaster item);

        [HttpDelete("/odata/Postgres/ItemMasters(MasterId={MasterId})")]
        public IActionResult DeleteItemMaster(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.ItemMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.ItemMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnItemMasterDeleted(item);
                this.context.ItemMasters.Remove(item);
                this.context.SaveChanges();
                this.OnAfterItemMasterDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnItemMasterUpdated(ERP.Server.Models.Postgres.ItemMaster item);
        partial void OnAfterItemMasterUpdated(ERP.Server.Models.Postgres.ItemMaster item);

        [HttpPut("/odata/Postgres/ItemMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutItemMaster(int key, [FromBody]ERP.Server.Models.Postgres.ItemMaster item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ItemMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.ItemMaster>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnItemMasterUpdated(item);
                this.context.ItemMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ItemMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "ItemGroupMaster,Master");
                this.OnAfterItemMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/ItemMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchItemMaster(int key, [FromBody]Delta<ERP.Server.Models.Postgres.ItemMaster> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ItemMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.ItemMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnItemMasterUpdated(item);
                this.context.ItemMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ItemMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "ItemGroupMaster,Master");
                this.OnAfterItemMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnItemMasterCreated(ERP.Server.Models.Postgres.ItemMaster item);
        partial void OnAfterItemMasterCreated(ERP.Server.Models.Postgres.ItemMaster item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.ItemMaster item)
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

                this.OnItemMasterCreated(item);
                this.context.ItemMasters.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ItemMasters.Where(i => i.MasterId == item.MasterId);

                Request.QueryString = Request.QueryString.Add("$expand", "ItemGroupMaster,Master");

                this.OnAfterItemMasterCreated(item);

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
