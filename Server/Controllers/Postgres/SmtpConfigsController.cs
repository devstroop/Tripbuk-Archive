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
    [Route("odata/Postgres/SmtpConfigs")]
    public partial class SmtpConfigsController : ODataController
    {
        private ERP.Server.Data.PostgresContext context;

        public SmtpConfigsController(ERP.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ERP.Server.Models.Postgres.SmtpConfig> GetSmtpConfigs()
        {
            var items = this.context.SmtpConfigs.AsQueryable<ERP.Server.Models.Postgres.SmtpConfig>();
            this.OnSmtpConfigsRead(ref items);

            return items;
        }

        partial void OnSmtpConfigsRead(ref IQueryable<ERP.Server.Models.Postgres.SmtpConfig> items);

        partial void OnSmtpConfigGet(ref SingleResult<ERP.Server.Models.Postgres.SmtpConfig> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/SmtpConfigs(Id={Id})")]
        public SingleResult<ERP.Server.Models.Postgres.SmtpConfig> GetSmtpConfig(Guid key)
        {
            var items = this.context.SmtpConfigs.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnSmtpConfigGet(ref result);

            return result;
        }
        partial void OnSmtpConfigDeleted(ERP.Server.Models.Postgres.SmtpConfig item);
        partial void OnAfterSmtpConfigDeleted(ERP.Server.Models.Postgres.SmtpConfig item);

        [HttpDelete("/odata/Postgres/SmtpConfigs(Id={Id})")]
        public IActionResult DeleteSmtpConfig(Guid key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.SmtpConfigs
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.SmtpConfig>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSmtpConfigDeleted(item);
                this.context.SmtpConfigs.Remove(item);
                this.context.SaveChanges();
                this.OnAfterSmtpConfigDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSmtpConfigUpdated(ERP.Server.Models.Postgres.SmtpConfig item);
        partial void OnAfterSmtpConfigUpdated(ERP.Server.Models.Postgres.SmtpConfig item);

        [HttpPut("/odata/Postgres/SmtpConfigs(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSmtpConfig(Guid key, [FromBody]ERP.Server.Models.Postgres.SmtpConfig item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SmtpConfigs
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.SmtpConfig>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnSmtpConfigUpdated(item);
                this.context.SmtpConfigs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SmtpConfigs.Where(i => i.Id == key);
                
                this.OnAfterSmtpConfigUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/SmtpConfigs(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchSmtpConfig(Guid key, [FromBody]Delta<ERP.Server.Models.Postgres.SmtpConfig> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.SmtpConfigs
                    .Where(i => i.Id == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ERP.Server.Models.Postgres.SmtpConfig>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnSmtpConfigUpdated(item);
                this.context.SmtpConfigs.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SmtpConfigs.Where(i => i.Id == key);
                
                this.OnAfterSmtpConfigUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnSmtpConfigCreated(ERP.Server.Models.Postgres.SmtpConfig item);
        partial void OnAfterSmtpConfigCreated(ERP.Server.Models.Postgres.SmtpConfig item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ERP.Server.Models.Postgres.SmtpConfig item)
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

                this.OnSmtpConfigCreated(item);
                this.context.SmtpConfigs.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.SmtpConfigs.Where(i => i.Id == item.Id);

                

                this.OnAfterSmtpConfigCreated(item);

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
