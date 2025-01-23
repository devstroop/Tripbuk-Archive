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
    [Route("odata/Postgres/AccountGroups")]
    public partial class AccountGroupsController : ODataController
    {
        private PostgresContext context;

        public AccountGroupsController(PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Server.Models.Postgres.AccountGroup> GetAccountGroups()
        {
            var items = this.context.AccountGroups.AsQueryable<Server.Models.Postgres.AccountGroup>();
            this.OnAccountGroupsRead(ref items);

            return items;
        }

        partial void OnAccountGroupsRead(ref IQueryable<Server.Models.Postgres.AccountGroup> items);

        partial void OnAccountGroupGet(ref SingleResult<Server.Models.Postgres.AccountGroup> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/AccountGroups(Id={Id})")]
        public SingleResult<Server.Models.Postgres.AccountGroup> GetAccountGroup(int key)
        {
            var items = this.context.AccountGroups.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnAccountGroupGet(ref result);

            return result;
        }
        partial void OnAccountGroupDeleted(Server.Models.Postgres.AccountGroup item);
        partial void OnAfterAccountGroupDeleted(Server.Models.Postgres.AccountGroup item);

        [HttpDelete("/odata/Postgres/AccountGroups(Id={Id})")]
        public IActionResult DeleteAccountGroup(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AccountGroups
                    .Where(i => i.Id == key)
                    .Include(i => i.AccountGroups1)
                    .Include(i => i.Accounts)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.AccountGroup>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAccountGroupDeleted(item);
                this.context.AccountGroups.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAccountGroupDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAccountGroupUpdated(Server.Models.Postgres.AccountGroup item);
        partial void OnAfterAccountGroupUpdated(Server.Models.Postgres.AccountGroup item);

        [HttpPut("/odata/Postgres/AccountGroups(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAccountGroup(int key, [FromBody]Server.Models.Postgres.AccountGroup item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AccountGroups
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.AccountGroup>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAccountGroupUpdated(item);
                this.context.AccountGroups.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountGroups.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroup1");
                this.OnAfterAccountGroupUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/AccountGroups(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAccountGroup(int key, [FromBody]Delta<Server.Models.Postgres.AccountGroup> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AccountGroups
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.AccountGroup>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAccountGroupUpdated(item);
                this.context.AccountGroups.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountGroups.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroup1");
                this.OnAfterAccountGroupUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAccountGroupCreated(Server.Models.Postgres.AccountGroup item);
        partial void OnAfterAccountGroupCreated(Server.Models.Postgres.AccountGroup item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Server.Models.Postgres.AccountGroup item)
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

                this.OnAccountGroupCreated(item);
                this.context.AccountGroups.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountGroups.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroup1");

                this.OnAfterAccountGroupCreated(item);

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
