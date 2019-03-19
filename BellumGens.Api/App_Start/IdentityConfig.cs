using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using BellumGens.Api.Models;
using BellumGens.Api.Providers;

namespace BellumGens.Api
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IIdentityMessageService emailService)
            : base(store)
        {
			EmailService = emailService;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<BellumGensDbContext>()), new EmailService());
			// Configure validation logic for usernames
			//manager.UserValidator = new UserValidator<ApplicationUser>(manager)
			//{
			//	AllowOnlyAlphanumericUserNames = false,
			//	RequireUniqueEmail = false
			//};
			//// Configure validation logic for passwords
			//manager.PasswordValidator = new PasswordValidator
			//{
			//    RequiredLength = 6,
			//    RequireNonLetterOrDigit = true,
			//    RequireDigit = true,
			//    RequireLowercase = true,
			//    RequireUppercase = true,
			//};
			var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

	public class EmailService : IIdentityMessageService
	{
		public Task SendAsync(IdentityMessage message)
		{
			return EmailServiceProvider.SendConfirmationEmail(message);
		}
	}
}
