using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult UpdateOrganisation()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ListPremises()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult CreatePremises()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult UpdatePremises()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
