using System.Web.Mvc;
using AlwaysEncryptedSample.Services;
using log4net;

namespace AlwaysEncryptedSample.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected ILog _log;
        protected readonly ApplicationDbContext _appContext = new ApplicationDbContext();

        protected ControllerBase()
        {
            _log = LogManager.GetLogger(GetType());
            _appContext.Database.Log = (dbLog => _log.Debug(dbLog));
            _log.DebugFormat("Initializing {0}", GetType().FullName);
        }
    }
}