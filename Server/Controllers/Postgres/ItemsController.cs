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
using Tripbuk.Server.Data;

namespace Tripbuk.Server.Controllers.Postgres
{
    [Route("odata/Postgres/Items")]
    public partial class ItemsController : ODataController
    {
        private PostgresContext context;

        public ItemsController(PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Server.Models.Postgres.Item> GetItems()
        {
            var items = this.context.Items.AsQueryable<Server.Models.Postgres.Item>();
            this.OnItemsRead(ref items);

            return items;
        }

        partial void OnItemsRead(ref IQueryable<Server.Models.Postgres.Item> items);

        partial void OnItemGet(ref SingleResult<Server.Models.Postgres.Item> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/Items(Id={Id})")]
        public SingleResult<Server.Models.Postgres.Item> GetItem(int key)
        {
            var items = this.context.Items.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnItemGet(ref result);

            return result;
        }
        partial void OnItemDeleted(Server.Models.Postgres.Item item);
        partial void OnAfterItemDeleted(Server.Models.Postgres.Item item);

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

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.Item>(Request, items);

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

        partial void OnItemUpdated(Server.Models.Postgres.Item item);
        partial void OnAfterItemUpdated(Server.Models.Postgres.Item item);

        [HttpPut("/odata/Postgres/Items(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutItem(int key, [FromBody]Server.Models.Postgres.Item item)
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

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.Item>(Request, items);

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
        public IActionResult PatchItem(int key, [FromBody]Delta<Server.Models.Postgres.Item> patch)
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

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.Item>(Request, items);

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

        partial void OnItemCreated(Server.Models.Postgres.Item item);
        partial void OnAfterItemCreated(Server.Models.Postgres.Item item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Server.Models.Postgres.Item item)
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
