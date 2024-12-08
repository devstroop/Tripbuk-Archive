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
    [Route("odata/Postgres/AccountMasters")]
    public partial class AccountMastersController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public AccountMastersController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.AccountMaster> GetAccountMasters()
        {
            var items = this.context.AccountMasters.AsQueryable<ERP.Server.Models.Postgres.AccountMaster>();
            this.OnAccountMastersRead(ref items);

            return items;
        }

        partial void OnAccountMastersRead(ref IQueryable<ERP.Server.Models.Postgres.AccountMaster> items);

        partial void OnAccountMasterGet(ref SingleResult<ERP.Server.Models.Postgres.AccountMaster> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/AccountMasters(MasterId={MasterId})")]
        public SingleResult<ERP.Server.Models.Postgres.AccountMaster> GetAccountMaster(int key)
        {
            var items = this.context.AccountMasters.Where(i => i.MasterId == key);
            var result = SingleResult.Create(items);

            OnAccountMasterGet(ref result);

            return result;
        }
        partial void OnAccountMasterDeleted(ERP.Server.Models.Postgres.AccountMaster item);
        partial void OnAfterAccountMasterDeleted(ERP.Server.Models.Postgres.AccountMaster item);

        [HttpDelete("/odata/Postgres/AccountMasters(MasterId={MasterId})")]
        public IActionResult DeleteAccountMaster(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.AccountMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.AccountMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAccountMasterDeleted(item);
                this.context.AccountMasters.Remove(item);
                this.context.SaveChanges();
                this.OnAfterAccountMasterDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAccountMasterUpdated(ERP.Server.Models.Postgres.AccountMaster item);
        partial void OnAfterAccountMasterUpdated(ERP.Server.Models.Postgres.AccountMaster item);

        [HttpPut("/odata/Postgres/AccountMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutAccountMaster(int key, [FromBody]ERP.Server.Models.Postgres.AccountMaster item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AccountMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.AccountMaster>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnAccountMasterUpdated(item);
                this.context.AccountMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroupMaster,Master");
                this.OnAfterAccountMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/AccountMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchAccountMaster(int key, [FromBody]Delta<ERP.Server.Models.Postgres.AccountMaster> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.AccountMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.AccountMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnAccountMasterUpdated(item);
                this.context.AccountMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroupMaster,Master");
                this.OnAfterAccountMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnAccountMasterCreated(ERP.Server.Models.Postgres.AccountMaster item);
        partial void OnAfterAccountMasterCreated(ERP.Server.Models.Postgres.AccountMaster item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.AccountMaster item)
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

                this.OnAccountMasterCreated(item);
                this.context.AccountMasters.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.AccountMasters.Where(i => i.MasterId == item.MasterId);

                Request.QueryString = Request.QueryString.Add("$expand", "AccountGroupMaster,Master");

                this.OnAfterAccountMasterCreated(item);

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
