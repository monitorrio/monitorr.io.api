using System.Web.Mvc;

namespace Web.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Contacts
        public ActionResult Index(string tab)
        {
            return View();
        }
    }
}