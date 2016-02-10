using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using AlwaysEncryptedSample.Models;
using AlwaysEncryptedSample.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ApplicationDbContext = AlwaysEncryptedSample.Services.ApplicationDbContext;

namespace AlwaysEncryptedSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Force creation of the database at startup.
            using (var authDbCtx = AuthDbContext.Create())
            {
                if (!authDbCtx.Roles.Any())
                {
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(authDbCtx));
                    roleManager.Create(new IdentityRole("DBAs"));
                    roleManager.Create(new IdentityRole("Credit Card Admins"));
                }
                if (!authDbCtx.Users.Any())
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(authDbCtx));
                    userManager.Create(new ApplicationUser
                    {
                        Id = "Administrator",
                        Email = "no-reply+admin@microsoft.com",
                        UserName = "Administrator",
                        EmailConfirmed = true,
                        PasswordHash = userManager.PasswordHasher.HashPassword("P3ter!"),
                    });

                    userManager.AddToRole("Administrator", "DBAs");

                    userManager.Create(new ApplicationUser
                    {
                        Email = "no-reply+creditcard@microsoft.com",
                        Id = "CCAdmin",
                        UserName = "CCAdmin",
                        EmailConfirmed = true,
                        PasswordHash = userManager.PasswordHasher.HashPassword("P@ul!")
                    });
                    userManager.AddToRole("CCAdmin", "Credit Card Admins");
                }
            }
            using (var context = new ApplicationDbContext())
            {
                //TODO: Could possibly be sped up, but its O(n^2) where n = 4
                foreach (var newCCN in CreditCardNetwork.GetNetworks())
                {
                    if (!context.CreditCardNetworks.Any(ccn => ccn.Id == newCCN.Id))
                    {
                        context.CreditCardNetworks.Add(newCCN);
                    }
                }
                context.SaveChanges();
            }
            
        }
    }
}
