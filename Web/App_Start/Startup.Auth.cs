using System;
using System.Web;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Web.Models;

namespace Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or Email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);


            var provider = new Auth0.Owin.Auth0AuthenticationProvider
            {
                OnReturnEndpoint = (context) =>
                {
                    // xsrf validation
                    if (context.Request.Query["state"] != null && context.Request.Query["state"].Contains("xsrf="))
                    {
                        var state = HttpUtility.ParseQueryString(context.Request.Query["state"]);
                        AntiForgery.Validate(context.Request.Cookies["__RequestVerificationToken"], state["xsrf"]);
                    }

                    return System.Threading.Tasks.Task.FromResult(0);
                }
            };

            app.UseAuth0Authentication(
                clientId: System.Configuration.ConfigurationManager.AppSettings["auth0:ClientId"],
                clientSecret: System.Configuration.ConfigurationManager.AppSettings["auth0:ClientSecret"],
                domain: System.Configuration.ConfigurationManager.AppSettings["auth0:Domain"],
                provider: provider);
        }
    }
}