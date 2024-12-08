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
    [Route("odata/Postgres/Items")]
    public partial class ItemsController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public ItemsController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.Item> GetItems()
        {
            var items = this.context.Items.AsQueryable<ERP.Server.Models.Postgres.Item>();
            this.OnItemsRead(ref items);

            return items;
        }

        partial void OnItemsRead(ref IQueryable<ERP.Server.Models.Postgres.Item> items);

        partial void OnItemGet(ref SingleResult<ERP.Server.Models.Postgres.Item> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/Items(Id={Id})")]
        public SingleResult<ERP.Server.Models.Postgres.Item> GetItem(int key)
        {
            var items = this.context.Items.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnItemGet(ref result);

            return result;
        }
        partial void OnItemDeleted(ERP.Server.Models.Postgres.Item item);
        partial void OnAfterItemDeleted(ERP.Server.Models.Postgres.Item item);

        [HttpDelete("/odata/Postgres/Items(Id={Id})")]
        public IActionResult DeleteItem(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Items
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.Item>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnItemDeleted(item);
                this.context.Items.Remove(item);
                this.context.SaveChanges();
                this.OnAfterItemDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnItemUpdated(ERP.Server.Models.Postgres.Item item);
        partial void OnAfterItemUpdated(ERP.Server.Models.Postgres.Item item);

        [HttpPut("/odata/Postgres/Items(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutItem(int key, [FromBody]ERP.Server.Models.Postgres.Item item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Items
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.Item>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnItemUpdated(item);
                this.context.Items.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Items.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "ItemGroup");
                this.OnAfterItemUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/Items(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchItem(int key, [FromBody]Delta<ERP.Server.Models.Postgres.Item> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Items
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.Item>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnItemUpdated(item);
                this.context.Items.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Items.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "ItemGroup");
                this.OnAfterItemUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnItemCreated(ERP.Server.Models.Postgres.Item item);
        partial void OnAfterItemCreated(ERP.Server.Models.Postgres.Item item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.Item item)
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

                this.OnItemCreated(item);
                this.context.Items.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Items.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "ItemGroup");

                this.OnAfterItemCreated(item);

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
