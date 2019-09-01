using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using BUS;
using System.Threading.Tasks;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        private TaskBUS _taskBus = new TaskBUS();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> TaskToMe()
        {
            var userName = User.Identity.GetUserName();
            var result = await _taskBus.TaskToMe(userName);
            return View(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> RequestToMe()
        {
            var userName = User.Identity.GetUserName();
            var result = await _taskBus.RequestToMe(userName);
            return View(result);
        }
    }
}
