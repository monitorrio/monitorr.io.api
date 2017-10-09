using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index(string id)
        {
            return View();
        }
    }
}