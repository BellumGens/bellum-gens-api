﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BellumGens.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Admin")]
    public class AdminController : BaseController
    {
        [Route("AppAdmin")]
        public bool GetUserIsAdmin()
        {
            return UserIsInRole("admin");
        }

        [Route("TournamentAdmin")]
        public bool GetUserIsTournamentAdmin()
        {
            return UserIsInRole("event-admin");
        }

        [HttpPut]
        [Route("CreateRole")]
        public async Task<IHttpActionResult> CreateRole(string rolename)
        {
            if (UserIsInRole("admin"))
            {
                var result = await RoleManager.CreateAsync(new IdentityRole() { Name = rolename }).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            return Unauthorized();
        }

        [Route("Roles")]
        public IHttpActionResult GetRoles()
        {
            if (UserIsInRole("admin"))
            {
                return Ok(RoleManager.Roles.Select(r => r.Name).ToList());
            }
            return Unauthorized();
        }

        [Route("Users")]
        public IHttpActionResult GetUsers()
        {
            if (UserIsInRole("admin"))
            {
                var users = _dbContext.Users.Select(s => new { s.Id, s.UserName, s.AvatarMedium, s.Roles }).ToList();
                return Ok(users);
            }
            return Unauthorized();
        }

        [Route("Promos")]
        public IHttpActionResult GetPromoCodes()
        {
            if (UserIsInRole("admin"))
            {
                var promos = _dbContext.PromoCodes.ToList();
                return Ok(promos);
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("AddUserRole")]
        public async Task<IHttpActionResult> AddUserToRole(string userid, string role)
        {
            if (UserIsInRole("admin"))
            {
                IdentityResult result = await UserManager.AddToRoleAsync(userid, role).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    return Ok("Ok");
                }
            }
            return Unauthorized();
        }
    }
}
