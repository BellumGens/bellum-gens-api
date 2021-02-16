using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin.Security.Providers.Steam;
using Owin.Security.Providers.Twitch;
using Owin;
using BellumGens.Api.Providers;
using BellumGens.Api.Models;
using Owin.Security.Providers.BattleNet;

namespace BellumGens.Api
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        private const bool allowInsecureHttp = true;

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(BellumGensDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
			{
				ExpireTimeSpan = TimeSpan.FromDays(30),
                CookieSameSite = SameSiteMode.None,
                CookieSecure = CookieSecureOption.SameAsRequest
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "bellum-gens-api";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = allowInsecureHttp
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);
		
			app.UseSteamAuthentication(AppInfo.Config.steamApiKey);

            var battleNetAuthOptions = new BattleNetAuthenticationOptions()
            {
                ClientId = AppInfo.Config.battleNetClientId,
                ClientSecret = AppInfo.Config.battleNetClientSecret,
                Region = Region.Europe
            };

            battleNetAuthOptions.Scope.Clear();
            battleNetAuthOptions.Scope.Add("sc2.profile");
            // battleNetAuthOptions.CallbackPath = new PathString("/signin-battlenet");

            app.UseBattleNetAuthentication(battleNetAuthOptions);

            var twitchAuthOptions = new TwitchAuthenticationOptions()
            {
                ClientId = AppInfo.Config.twitchClientId,
                ClientSecret = AppInfo.Config.twitchSecret
            };

            app.UseTwitchAuthentication(twitchAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
