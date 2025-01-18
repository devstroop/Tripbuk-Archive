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
    [Route("odata/Postgres/AccountGroups")]
    public partial class AccountGroupsController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public AccountGroupsController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.AccountGroup> GetAccountGroups()
        {
            var items = this.context.AccountGroups.AsQueryable<ERP.Server.Models.Postgres.AccountGroup>();
            this.OnAccountGroupsRead(ref items);

            return items;
        }

        partial void OnAccountGroupsRead(ref IQueryable<ERP.Server.Models.Postgres.AccountGroup> items);

        partial void OnAccountGroupGet(ref SingleResult<ERP.Server.Models.Postgres.AccountGroup> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/AccountGroups(Id={Id})")]
        public SingleResult<ERP.Server.Models.Postgres.AccountGroup> GetAccountGroup(int key)
        {
            var items = this.context.AccountGroups.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnAccountGroupGet(ref result);

            return result;
        }
        partial void OnAccountGroupDeleted(ERP.Server.Models.Postgres.AccountGroup item);
        partial void OnAfterAccountGroupDeleted(ERP.Server.Models.Postgres.AccountGroup item);

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

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.AccountGroup>(Request, items);

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

        partial void OnAccountGroupUpdated(ERP.Server.Models.Postgres.AccountGroup item);
        partial void OnAfterAccountGroupUpdated(ERP.Server.Models.Postgres.AccountGroup item);

        [HttpPut("/odata/Postgres/AccountGroups(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAccountGroup(int key, [FromBody]ERP.Server.Models.Postgres.AccountGroup item)
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

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.AccountGroup>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAccountGroupUpdated(item);
                this.context.AccountGroups.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountGroups.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroup1,AspNetTenant");
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
        public IActionResult PatchAccountGroup(int key, [FromBody]Delta<ERP.Server.Models.Postgres.AccountGroup> patch)
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

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.AccountGroup>(Request, items);

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
                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroup1,AspNetTenant");
                this.OnAfterAccountGroupUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAccountGroupCreated(ERP.Server.Models.Postgres.AccountGroup item);
        partial void OnAfterAccountGroupCreated(ERP.Server.Models.Postgres.AccountGroup item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.AccountGroup item)
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

                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroup1,AspNetTenant");

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
