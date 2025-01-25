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

namespace Tripbuk.Server.Controllers.Postgres
{
    [Route("odata/Postgres/Tags")]
    public partial class TagsController : ODataController
    {
        private Tripbuk.Server.Data.PostgresContext context;

        public TagsController(Tripbuk.Server.Data.PostgresContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<Tripbuk.Server.Models.Postgres.Tag> GetTags()
        {
            var items = this.context.Tags.AsQueryable<Tripbuk.Server.Models.Postgres.Tag>();
            this.OnTagsRead(ref items);

            return items;
        }

        partial void OnTagsRead(ref IQueryable<Tripbuk.Server.Models.Postgres.Tag> items);

        partial void OnTagGet(ref SingleResult<Tripbuk.Server.Models.Postgres.Tag> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/Postgres/Tags(Id={Id})")]
        public SingleResult<Tripbuk.Server.Models.Postgres.Tag> GetTag(int key)
        {
            var items = this.context.Tags.Where(i => i.Id == key);
            var result = SingleResult.Create(items);

            OnTagGet(ref result);

            return result;
        }
        partial void OnTagDeleted(Tripbuk.Server.Models.Postgres.Tag item);
        partial void OnAfterTagDeleted(Tripbuk.Server.Models.Postgres.Tag item);

        [HttpDelete("/odata/Postgres/Tags(Id={Id})")]
        public IActionResult DeleteTag(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Tags
                    .Where(i => i.Id == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTagDeleted(item);
                this.context.Tags.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTagDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTagUpdated(Tripbuk.Server.Models.Postgres.Tag item);
        partial void OnAfterTagUpdated(Tripbuk.Server.Models.Postgres.Tag item);

        [HttpPut("/odata/Postgres/Tags(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTag(int key, [FromBody]Tripbuk.Server.Models.Postgres.Tag item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Id != key))
                {
                    return BadRequest();
                }
                this.OnTagUpdated(item);
                this.context.Tags.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Tags.Where(i => i.Id == key);
                
                this.OnAfterTagUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/Postgres/Tags(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTag(int key, [FromBody]Delta<Tripbuk.Server.Models.Postgres.Tag> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Tags.Where(i => i.Id == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTagUpdated(item);
                this.context.Tags.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Tags.Where(i => i.Id == key);
                
                this.OnAfterTagUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTagCreated(Tripbuk.Server.Models.Postgres.Tag item);
        partial void OnAfterTagCreated(Tripbuk.Server.Models.Postgres.Tag item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] Tripbuk.Server.Models.Postgres.Tag item)
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

                this.OnTagCreated(item);
                this.context.Tags.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Tags.Where(i => i.Id == item.Id);

                

                this.OnAfterTagCreated(item);

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
