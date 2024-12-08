using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using System.ComponentModel.DataAnnotations.Schema;

using ERP.Server.Data;
using ERP.Server.Models;

namespace ERP.Server.Controllers
{
    [Authorize]
    [Route("odata/Identity/ApplicationUsers")]
    public partial class ApplicationUsersController : ODataController
    {
        private readonly ApplicationIdentityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public ApplicationUsersController(ApplicationIdentityDbContext context, UserManager<ApplicationUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        partial void OnUsersRead(ref IQueryable<ApplicationUser> users);

        [EnableQuery]
        [HttpGet]
        public IEnumerable<ApplicationUser> Get()
        {
            var users = _userManager.Users;
            OnUsersRead(ref users);

            return users;
        }

        [EnableQuery]
        [HttpGet("{Id}")]
        public SingleResult<ApplicationUser> GetApplicationUser(string key)
        {
            var user = _context.Users.Where(i => i.Id == key);

            return SingleResult.Create(user);
        }

        partial void OnUserDeleted(ApplicationUser user);

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string key)
        {
            var user = await _userManager.FindByIdAsync(key);

            if (user == null)
            {
                return NotFound();
            }

            OnUserDeleted(user);

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return IdentityError(result);
            }

            return new NoContentResult();
        }

        partial void OnUserUpdated(ApplicationUser user);

        [HttpPatch("{Id}")]
        public async Task<IActionResult> Patch(string key, [FromBody]ApplicationUser data)
        {
            var user = await _userManager.FindByIdAsync(key);

            if (user == null)
            {
                return NotFound();
            }

            OnUserUpdated(data);

            IdentityResult result = null;

            user.Roles = null;

            result = await _userManager.UpdateAsync(user);

            if (data.Roles != null)
            {
                result = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

                if (result.Succeeded) 
                {
                    result = await _userManager.AddToRolesAsync(user, data.Roles.Select(r => r.Name));
                }
            }

            if (!string.IsNullOrEmpty(data.Password))
            {
                result = await _userManager.RemovePasswordAsync(user);

                if (result.Succeeded)
                {
                    result = await _userManager.AddPasswordAsync(user, data.Password);
                }

                if (!result.Succeeded)
                {
                    return IdentityError(result);
                }
            }

            if (result != null && !result.Succeeded)
            {
                return IdentityError(result);
            }

            return new NoContentResult();
        }

        partial void OnUserCreated(ApplicationUser user);

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApplicationUser user)
        {
            user.UserName = user.Email;
            user.EmailConfirmed = true;
            var password = user.Password;
            var roles = user.Roles;
            user.Roles = null;
            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded && roles != null)
            {
                result = await _userManager.AddToRolesAsync(user, roles.Select(r => r.Name));
            }

            user.Roles = roles;

            if (result.Succeeded)
            {
                OnUserCreated(user);

                return Created($"odata/Identity/Users('{user.Id}')", user);
            }
            else
            {
                return IdentityError(result);
            }
        }

        private IActionResult IdentityError(IdentityResult result)
        {
            var message = string.Join(", ", result.Errors.Select(error => error.Description));

            return BadRequest(new { error = new { message } });
        }
    }
}