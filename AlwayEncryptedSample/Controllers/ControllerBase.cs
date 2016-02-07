using System.Web.Mvc;
using AlwayEncryptedSample.Services;

namespace AlwayEncryptedSample.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected readonly ApplicationDbContext _appContext = new ApplicationDbContext();
    }
}