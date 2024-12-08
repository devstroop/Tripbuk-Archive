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
    [Route("odata/Postgres/AccountGroupMasters")]
    public partial class AccountGroupMastersController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public AccountGroupMastersController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.AccountGroupMaster> GetAccountGroupMasters()
        {
            var items = this.context.AccountGroupMasters.AsQueryable<ERP.Server.Models.Postgres.AccountGroupMaster>();
            this.OnAccountGroupMastersRead(ref items);

            return items;
        }

        partial void OnAccountGroupMastersRead(ref IQueryable<ERP.Server.Models.Postgres.AccountGroupMaster> items);

        partial void OnAccountGroupMasterGet(ref SingleResult<ERP.Server.Models.Postgres.AccountGroupMaster> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/AccountGroupMasters(MasterId={MasterId})")]
        public SingleResult<ERP.Server.Models.Postgres.AccountGroupMaster> GetAccountGroupMaster(int key)
        {
            var items = this.context.AccountGroupMasters.Where(i => i.MasterId == key);
            var result = SingleResult.Create(items);

            OnAccountGroupMasterGet(ref result);

            return result;
        }
        partial void OnAccountGroupMasterDeleted(ERP.Server.Models.Postgres.AccountGroupMaster item);
        partial void OnAfterAccountGroupMasterDeleted(ERP.Server.Models.Postgres.AccountGroupMaster item);

        [HttpDelete("/odata/Postgres/AccountGroupMasters(MasterId={MasterId})")]
        public IActionResult DeleteAccountGroupMaster(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AccountGroupMasters
                    .Where(i => i.MasterId == key)
                    .Include(i => i.AccountGroupMasters1)
                    .Include(i => i.AccountMasters)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.AccountGroupMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAccountGroupMasterDeleted(item);
                this.context.AccountGroupMasters.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAccountGroupMasterDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAccountGroupMasterUpdated(ERP.Server.Models.Postgres.AccountGroupMaster item);
        partial void OnAfterAccountGroupMasterUpdated(ERP.Server.Models.Postgres.AccountGroupMaster item);

        [HttpPut("/odata/Postgres/AccountGroupMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAccountGroupMaster(int key, [FromBody]ERP.Server.Models.Postgres.AccountGroupMaster item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AccountGroupMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.AccountGroupMaster>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAccountGroupMasterUpdated(item);
                this.context.AccountGroupMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountGroupMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Master,AccountGroupMaster1");
                this.OnAfterAccountGroupMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/AccountGroupMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAccountGroupMaster(int key, [FromBody]Delta<ERP.Server.Models.Postgres.AccountGroupMaster> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AccountGroupMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.AccountGroupMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAccountGroupMasterUpdated(item);
                this.context.AccountGroupMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountGroupMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Master,AccountGroupMaster1");
                this.OnAfterAccountGroupMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAccountGroupMasterCreated(ERP.Server.Models.Postgres.AccountGroupMaster item);
        partial void OnAfterAccountGroupMasterCreated(ERP.Server.Models.Postgres.AccountGroupMaster item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.AccountGroupMaster item)
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

                this.OnAccountGroupMasterCreated(item);
                this.context.AccountGroupMasters.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountGroupMasters.Where(i => i.MasterId == item.MasterId);

                Request.QueryString = Request.QueryString.Add("$expand", "Master,AccountGroupMaster1");

                this.OnAfterAccountGroupMasterCreated(item);

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
