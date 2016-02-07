using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AlwayEncryptedSample.Models;
using AlwayEncryptedSample.Services;
using ApplicationDbContext = AlwayEncryptedSample.Services.ApplicationDbContext;

namespace AlwayEncryptedSample
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
            AuthDbContext.Create().Users.Find("");
            (new ApplicationDbContext()).CreditCards.Find(-1);
        }
    }
}
