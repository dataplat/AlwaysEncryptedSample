using System.Web.Mvc;
using AlwaysEncryptedSample.Models;
using log4net;

namespace AlwaysEncryptedSample.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected ILog _log;
        protected readonly ApplicationDbContext _appContext = ApplicationDbContext.Create();

        protected ControllerBase()
        {
            _log = LogManager.GetLogger(GetType());
            _appContext.Database.Log = (dbLog => _log.Debug(dbLog));
            _log.DebugFormat("Initializing {0}", GetType().FullName);
        }
    }
}