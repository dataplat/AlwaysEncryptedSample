using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
//using System.Resources;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using AlwaysEncryptedSample.Models;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AlwaysEncryptedSample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                // CreateDatabase will fail on azure because I can't connect to tempdb;
                var builder = new SqlConnectionStringBuilder(cn.ConnectionString);
                var createDbError = DbInit.CreateDatabase(builder);
                DbInit.InitLog4NetDb(cn);
            }
            
            var log = LogManager.GetLogger(GetType());
            log.Info("Calling Application_Start");
            
            log.Debug("Performing Area Registration");
            AreaRegistration.RegisterAllAreas();
            log.Debug("Performing Global Configuration");
            GlobalConfiguration.Configure(WebApiConfig.Register);
            log.Debug("Registering GLobal Filters");
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            log.Debug("Registering Routes");
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            log.Debug("Registering Bundles");
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Force creation of auth schema.
            DbInit.CreateAuthContext();
            // Force creation of Purchasing schema.
            DbInit.CreatePurchasingContext();
            log.InfoFormat("Application_Start exiting");
        }

        
    }
}
