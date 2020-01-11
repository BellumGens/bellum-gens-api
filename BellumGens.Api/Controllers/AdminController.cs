using System;
using System.Linq;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BellumGens.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [RoutePrefix("api/Admin")]
    public class AdminController : BaseController
    {
        [AllowAnonymous]
        [Route("AppAdmin")]
        public bool GetUserIsAdmin()
        {
            return User.Identity.IsAuthenticated && User.IsInRole("admin");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("CreateRole")]
        public IHttpActionResult CreateRole(string rolename)
        {
            _dbContext.Roles.Add(new IdentityRole()
            {
                Name = rolename
            });

            _dbContext.SaveChanges();
            return Ok();
        }

        [AllowAnonymous]
        [Route("Roles")]
        public IHttpActionResult GetRoles()
        {
            return Ok(_dbContext.Roles.ToList());
        }
    }
}
