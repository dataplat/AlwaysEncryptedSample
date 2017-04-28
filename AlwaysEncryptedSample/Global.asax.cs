using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Resources;
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
        private void InitLog4NetDb(SqlConnection cn)
        {
            //TODO: Make the database if it doesn't exist.
            var rm = new ResourceManager
                ("AlwaysEncryptedSample.Properties.Resources", Assembly.GetExecutingAssembly());
            var sql = rm.GetString("Log4NetDDL");
            using (var cmd = cn.CreateCommand())
            {
                /*
                 * EF Code first does a good job of autocreating the database if it doesn't
                 * exist. However, since we want log4net to log EF activity to the database
                 * we have a chicken or egg problem.
                 */
                var builder = new SqlConnectionStringBuilder(cn.ConnectionString);
                var dbName = builder.InitialCatalog;
                // Can't connect to a db that doesn't already exist. 
                builder.InitialCatalog = "tempdb";
                cn.ConnectionString = builder.ConnectionString;
                cn.Open();
                cmd.CommandText = 
                    "IF DB_ID(@dbName) IS NULL " +
                    "EXEC ('CREATE DATABASE ' + @dbName + '; " +
                    "ALTER DATABASE ' + @dbName + ' SET RECOVERY SIMPLE;')";
                cmd.Parameters.AddWithValue("@dbName", dbName);
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                cn.Close();
                builder.InitialCatalog = dbName;
                cn.ConnectionString = builder.ConnectionString;
                cn.Open();
                /*
                 * ADO.NET doesn't parse batches (read support the GO statement).
                 * Because CREATE SCHEMA is extra picky we need this.
                 */
                cmd.CommandText = "SELECT COUNT(*) FROM sys.schemas WHERE name = 'Logging';";
                if ((int) cmd.ExecuteScalar() == 0)
                {
                    cmd.CommandText = "CREATE SCHEMA [Logging]";
                    cmd.ExecuteNonQuery();
                }
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                cn.Close();
            }
        }
        protected void Application_Start()
        {
            using (var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                InitLog4NetDb(cn);
            }
            
            var log = log4net.LogManager.GetLogger(GetType());
            log.InfoFormat("Calling Application_Start");
            
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
            using (var authDbCtx = AuthDbContext.Create())
            {
                authDbCtx.Database.Log = (dbLog => log.Debug(dbLog));
                log.Info("Initialization tests for Autorization Schema");
                if (!authDbCtx.Roles.Any())
                {
                    log.Info("No roles found in database. Creating roles");
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(authDbCtx));
                    roleManager.Create(new IdentityRole("DBAs")); // Gives database internals access
                    roleManager.Create(new IdentityRole("Credit Card Admins")); // Gives access to CC info
                }
                if (!authDbCtx.Users.Any())
                {
                    log.Info("No users found in database. Creating users");
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(authDbCtx));
                    userManager.Create(new ApplicationUser
                    {
                        Id = "Administrator",
                        Email = "no-reply+admin@microsoft.com",
                        UserName = "Administrator",
                        EmailConfirmed = true,
                        //PasswordHash = userManager.PasswordHasher.HashPassword("P3ter!"),
                        PasswordHash = userManager.PasswordHasher.HashPassword("Alm0nds!"),
                    });

                    userManager.AddToRole("Administrator", "DBAs");

                    userManager.Create(new ApplicationUser
                    {
                        Email = "no-reply+creditcard@microsoft.com",
                        Id = "CCAdmin",
                        UserName = "CCAdmin",
                        EmailConfirmed = true,
                        //PasswordHash = userManager.PasswordHasher.HashPassword("P@ul!")
                        PasswordHash = userManager.PasswordHasher.HashPassword("Appl3s")
                    });
                    userManager.AddToRole("CCAdmin", "Credit Card Admins");
                }
            }
            // Force creation of app schema.
            using (var context = new ApplicationDbContext())
            {
                // TODO: Perhaps rethink doing this.
                context.Database.Log = (dbLog => log.Debug(dbLog));
                log.Info("Initialization tests for Application Schema");
                //TODO: Could probably be sped up, but its O(n^2) where n = 4
                foreach (var newCCN in CreditCardNetwork.GetNetworks())
                {
                    if (!context.CreditCardNetworks.Any(ccn => ccn.Id == newCCN.Id))
                    {
                        context.CreditCardNetworks.Add(newCCN);
                    }
                }
                context.SaveChanges();
            }
            log.InfoFormat("Application_Start exiting");
        }
    }
}
