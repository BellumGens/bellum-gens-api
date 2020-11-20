using BellumGens.Api.Core.Models;
using BellumGens.Api.Core.Models.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BellumGens.Api.Controllers
{
	[ApiController]
	public class BaseController : ControllerBase
    {
		protected readonly IEmailSender _sender;
		protected readonly BellumGensDbContext _dbContext;
		protected readonly UserManager<ApplicationUser> _userManager;
		protected readonly RoleManager<IdentityRole> _roleManager;

		public BaseController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender sender, BellumGensDbContext context)
        {
			_userManager = userManager;
			_roleManager = roleManager;
			_sender = sender;
			_dbContext = context;
        }

		protected async Task<ApplicationUser> GetAuthUser()
		{
			return User.Identity.IsAuthenticated ? await _userManager.FindByIdAsync(User.GetResolvedUserId()) : null;
		}

		protected async Task<bool> UserIsInRole(string role)
		{
			return await _userManager.IsInRoleAsync(await GetAuthUser(), role);
		}
	}
}
