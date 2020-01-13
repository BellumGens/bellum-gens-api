using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BellumGens.Api.Providers;
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
            return UserManager.IsInRole(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()), "admin");
        }

        [Route("TournamentAdmin")]
        public bool GetUserIsTournamentAdmin()
        {
            return UserManager.IsInRole(SteamServiceProvider.SteamUserId(User.Identity.GetUserId()), "event-admin");
        }

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("CreateRole")]
        //public async Task<IHttpActionResult> CreateRole(string rolename)
        //{
        //    var result = await RoleManager.CreateAsync(new IdentityRole() { Name = rolename }).ConfigureAwait(false);
        //    if (result.Succeeded)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest();
        //}

        //[AllowAnonymous]
        //[Route("Roles")]
        //public IHttpActionResult GetRoles()
        //{
        //    return Ok(RoleManager.Roles.Select(r => r.Name).ToList());
        //}

        //[AllowAnonymous]
        //[Route("Users")]
        //public IHttpActionResult GetUsers()
        //{
        //    return Ok(UserManager.Users.ToList());
        //}

        //[AllowAnonymous]
        //[HttpGet]
        //[Route("AddUserRole")]
        //public async Task<IHttpActionResult> AddUserRoles(string userid, string role)
        //{
        //    IdentityResult result = await UserManager.AddToRoleAsync(userid, role).ConfigureAwait(false);
        //    if (result.Succeeded)
        //    {
        //        return Ok("Ok");
        //    }
        //    return BadRequest("Something went wrong");
        //}
    }
}
