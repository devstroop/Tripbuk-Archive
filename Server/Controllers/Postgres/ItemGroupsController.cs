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
    [Route("odata/Postgres/ItemGroups")]
    public partial class ItemGroupsController : ODataController
    {
        private PostgresContext context;

        public ItemGroupsController(PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Server.Models.Postgres.ItemGroup> GetItemGroups()
        {
            var items = this.context.ItemGroups.AsQueryable<Server.Models.Postgres.ItemGroup>();
            this.OnItemGroupsRead(ref items);

            return items;
        }

        partial void OnItemGroupsRead(ref IQueryable<Server.Models.Postgres.ItemGroup> items);

        partial void OnItemGroupGet(ref SingleResult<Server.Models.Postgres.ItemGroup> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/ItemGroups(Id={Id})")]
        public SingleResult<Server.Models.Postgres.ItemGroup> GetItemGroup(int key)
        {
            var items = this.context.ItemGroups.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnItemGroupGet(ref result);

            return result;
        }
        partial void OnItemGroupDeleted(Server.Models.Postgres.ItemGroup item);
        partial void OnAfterItemGroupDeleted(Server.Models.Postgres.ItemGroup item);

        [HttpDelete("/odata/Postgres/ItemGroups(Id={Id})")]
        public IActionResult DeleteItemGroup(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.ItemGroups
                    .Where(i => i.Id == key)
                    .Include(i => i.ItemGroups1)
                    .Include(i => i.Items)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.ItemGroup>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnItemGroupDeleted(item);
                this.context.ItemGroups.Remove(item);
                this.context.SaveChanges();
                this.OnAfterItemGroupDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnItemGroupUpdated(Server.Models.Postgres.ItemGroup item);
        partial void OnAfterItemGroupUpdated(Server.Models.Postgres.ItemGroup item);

        [HttpPut("/odata/Postgres/ItemGroups(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutItemGroup(int key, [FromBody]Server.Models.Postgres.ItemGroup item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ItemGroups
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.ItemGroup>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnItemGroupUpdated(item);
                this.context.ItemGroups.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ItemGroups.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "ItemGroup1");
                this.OnAfterItemGroupUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/ItemGroups(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchItemGroup(int key, [FromBody]Delta<Server.Models.Postgres.ItemGroup> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ItemGroups
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.ItemGroup>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnItemGroupUpdated(item);
                this.context.ItemGroups.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ItemGroups.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "ItemGroup1");
                this.OnAfterItemGroupUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnItemGroupCreated(Server.Models.Postgres.ItemGroup item);
        partial void OnAfterItemGroupCreated(Server.Models.Postgres.ItemGroup item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Server.Models.Postgres.ItemGroup item)
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

                this.OnItemGroupCreated(item);
                this.context.ItemGroups.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ItemGroups.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "ItemGroup1");

                this.OnAfterItemGroupCreated(item);

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
