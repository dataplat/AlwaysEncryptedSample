using System.Web.Mvc;
using AlwaysEncryptedSample.Services;

namespace AlwaysEncryptedSample.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected readonly ApplicationDbContext _appContext = new ApplicationDbContext();
    }
}