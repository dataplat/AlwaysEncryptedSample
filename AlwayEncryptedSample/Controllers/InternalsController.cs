using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web.Mvc;
using AlwayEncryptedSample.Models;

namespace AlwayEncryptedSample.Controllers
{
    /// <summary>
    /// Internals for the DBA to see
    /// </summary>
    [Authorize(Roles="DBAs")]
    public sealed class InternalsController : ControllerBase
    {
        private ResourceManager _rm = new ResourceManager("AlwayEncryptedSample.Properties.Resources", Assembly.GetExecutingAssembly());

        public ActionResult Index()
        {
            var sql = _rm.GetString("EncryptedColumnsSQL");
            var columns = _appContext.Database.SqlQuery<ColumnInfo>(sql);

            return View(columns.ToList());
        }
    }
}