using Northwind.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace DataModelClasses.Controllers
{
    public class HomeController : ControllerAutoMapperBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignedOut", "Home");
        }

        public ActionResult SignedOut()
        {
            return View();
        }
    }
}