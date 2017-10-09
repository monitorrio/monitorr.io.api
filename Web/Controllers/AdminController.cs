using System.Web.Mvc;
using SharpAuth0;

namespace Web.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {

        public AdminController(IIdentityGateway identityGateway) : base(identityGateway)
        {
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Profile(int id, string tab)
        {
            return View();
        }
        public ActionResult UserManagement(string tab)
        {
            return View();
        }
        public ActionResult Reports()
        {
            return View();
        }
        public ActionResult Tools()
        {
            return View();
        }
    }
}