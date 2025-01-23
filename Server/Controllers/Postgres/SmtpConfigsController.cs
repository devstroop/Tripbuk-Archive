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
    [Route("odata/Postgres/SmtpConfigs")]
    public partial class SmtpConfigsController : ODataController
    {
        private PostgresContext context;

        public SmtpConfigsController(PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Server.Models.Postgres.SmtpConfig> GetSmtpConfigs()
        {
            var items = this.context.SmtpConfigs.AsQueryable<Server.Models.Postgres.SmtpConfig>();
            this.OnSmtpConfigsRead(ref items);

            return items;
        }

        partial void OnSmtpConfigsRead(ref IQueryable<Server.Models.Postgres.SmtpConfig> items);

        partial void OnSmtpConfigGet(ref SingleResult<Server.Models.Postgres.SmtpConfig> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/SmtpConfigs(Id={Id})")]
        public SingleResult<Server.Models.Postgres.SmtpConfig> GetSmtpConfig(Guid key)
        {
            var items = this.context.SmtpConfigs.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnSmtpConfigGet(ref result);

            return result;
        }
        partial void OnSmtpConfigDeleted(Server.Models.Postgres.SmtpConfig item);
        partial void OnAfterSmtpConfigDeleted(Server.Models.Postgres.SmtpConfig item);

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

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.SmtpConfig>(Request, items);

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

        partial void OnSmtpConfigUpdated(Server.Models.Postgres.SmtpConfig item);
        partial void OnAfterSmtpConfigUpdated(Server.Models.Postgres.SmtpConfig item);

        [HttpPut("/odata/Postgres/SmtpConfigs(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutSmtpConfig(Guid key, [FromBody]Server.Models.Postgres.SmtpConfig item)
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

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.SmtpConfig>(Request, items);

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
        public IActionResult PatchSmtpConfig(Guid key, [FromBody]Delta<Server.Models.Postgres.SmtpConfig> patch)
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

                items = Data.EntityPatch.ApplyTo<Server.Models.Postgres.SmtpConfig>(Request, items);

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

        partial void OnSmtpConfigCreated(Server.Models.Postgres.SmtpConfig item);
        partial void OnAfterSmtpConfigCreated(Server.Models.Postgres.SmtpConfig item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Server.Models.Postgres.SmtpConfig item)
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
