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
    [Route("odata/Postgres/StdNarrationMasters")]
    public partial class StdNarrationMastersController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public StdNarrationMastersController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.StdNarrationMaster> GetStdNarrationMasters()
        {
            var items = this.context.StdNarrationMasters.AsQueryable<ERP.Server.Models.Postgres.StdNarrationMaster>();
            this.OnStdNarrationMastersRead(ref items);

            return items;
        }

        partial void OnStdNarrationMastersRead(ref IQueryable<ERP.Server.Models.Postgres.StdNarrationMaster> items);

        partial void OnStdNarrationMasterGet(ref SingleResult<ERP.Server.Models.Postgres.StdNarrationMaster> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/StdNarrationMasters(MasterId={MasterId})")]
        public SingleResult<ERP.Server.Models.Postgres.StdNarrationMaster> GetStdNarrationMaster(int key)
        {
            var items = this.context.StdNarrationMasters.Where(i => i.MasterId == key);
            var result = SingleResult.Create(items);

            OnStdNarrationMasterGet(ref result);

            return result;
        }
        partial void OnStdNarrationMasterDeleted(ERP.Server.Models.Postgres.StdNarrationMaster item);
        partial void OnAfterStdNarrationMasterDeleted(ERP.Server.Models.Postgres.StdNarrationMaster item);

        [HttpDelete("/odata/Postgres/StdNarrationMasters(MasterId={MasterId})")]
        public IActionResult DeleteStdNarrationMaster(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.StdNarrationMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.StdNarrationMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStdNarrationMasterDeleted(item);
                this.context.StdNarrationMasters.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStdNarrationMasterDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStdNarrationMasterUpdated(ERP.Server.Models.Postgres.StdNarrationMaster item);
        partial void OnAfterStdNarrationMasterUpdated(ERP.Server.Models.Postgres.StdNarrationMaster item);

        [HttpPut("/odata/Postgres/StdNarrationMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStdNarrationMaster(int key, [FromBody]ERP.Server.Models.Postgres.StdNarrationMaster item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StdNarrationMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.StdNarrationMaster>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStdNarrationMasterUpdated(item);
                this.context.StdNarrationMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StdNarrationMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Master");
                this.OnAfterStdNarrationMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/StdNarrationMasters(MasterId={MasterId})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStdNarrationMaster(int key, [FromBody]Delta<ERP.Server.Models.Postgres.StdNarrationMaster> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.StdNarrationMasters
                    .Where(i => i.MasterId == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.StdNarrationMaster>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStdNarrationMasterUpdated(item);
                this.context.StdNarrationMasters.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StdNarrationMasters.Where(i => i.MasterId == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Master");
                this.OnAfterStdNarrationMasterUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStdNarrationMasterCreated(ERP.Server.Models.Postgres.StdNarrationMaster item);
        partial void OnAfterStdNarrationMasterCreated(ERP.Server.Models.Postgres.StdNarrationMaster item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.StdNarrationMaster item)
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

                this.OnStdNarrationMasterCreated(item);
                this.context.StdNarrationMasters.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.StdNarrationMasters.Where(i => i.MasterId == item.MasterId);

                Request.QueryString = Request.QueryString.Add("$expand", "Master");

                this.OnAfterStdNarrationMasterCreated(item);

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
