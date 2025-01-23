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

namespace TripBUK.Server.Controllers.Postgres
{
    [Route("odata/Postgres/Accounts")]
    public partial class AccountsController : ODataController
    {
        private TripBUK.Server.Data.PostgresContext context;

        public AccountsController(TripBUK.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<TripBUK.Server.Models.Postgres.Account> GetAccounts()
        {
            var items = this.context.Accounts.AsQueryable<TripBUK.Server.Models.Postgres.Account>();
            this.OnAccountsRead(ref items);

            return items;
        }

        partial void OnAccountsRead(ref IQueryable<TripBUK.Server.Models.Postgres.Account> items);

        partial void OnAccountGet(ref SingleResult<TripBUK.Server.Models.Postgres.Account> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/Accounts(Id={Id})")]
        public SingleResult<TripBUK.Server.Models.Postgres.Account> GetAccount(int key)
        {
            var items = this.context.Accounts.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnAccountGet(ref result);

            return result;
        }
        partial void OnAccountDeleted(TripBUK.Server.Models.Postgres.Account item);
        partial void OnAfterAccountDeleted(TripBUK.Server.Models.Postgres.Account item);

        [HttpDelete("/odata/Postgres/Accounts(Id={Id})")]
        public IActionResult DeleteAccount(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Accounts
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<TripBUK.Server.Models.Postgres.Account>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAccountDeleted(item);
                this.context.Accounts.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAccountDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAccountUpdated(TripBUK.Server.Models.Postgres.Account item);
        partial void OnAfterAccountUpdated(TripBUK.Server.Models.Postgres.Account item);

        [HttpPut("/odata/Postgres/Accounts(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAccount(int key, [FromBody]TripBUK.Server.Models.Postgres.Account item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Accounts
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<TripBUK.Server.Models.Postgres.Account>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAccountUpdated(item);
                this.context.Accounts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Accounts.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroup");
                this.OnAfterAccountUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/Accounts(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAccount(int key, [FromBody]Delta<TripBUK.Server.Models.Postgres.Account> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Accounts
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<TripBUK.Server.Models.Postgres.Account>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAccountUpdated(item);
                this.context.Accounts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Accounts.Where(i => i.Id == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroup");
                this.OnAfterAccountUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAccountCreated(TripBUK.Server.Models.Postgres.Account item);
        partial void OnAfterAccountCreated(TripBUK.Server.Models.Postgres.Account item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] TripBUK.Server.Models.Postgres.Account item)
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

                this.OnAccountCreated(item);
                this.context.Accounts.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Accounts.Where(i => i.Id == item.Id);

                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroup");

                this.OnAfterAccountCreated(item);

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
