using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using RecruitmentAgency;
using RecruitmentAgency.Models;
using RecruitmentAgency.Models.Identity;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace RecruitmentAgency
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new UsersManager(new DatabaseContext().Users));
            app.CreatePerOwinContext<SignInManager>((options, context) => new SignInManager(context.GetUserManager<UsersManager>(), context.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider()
            });
        }
    }
}