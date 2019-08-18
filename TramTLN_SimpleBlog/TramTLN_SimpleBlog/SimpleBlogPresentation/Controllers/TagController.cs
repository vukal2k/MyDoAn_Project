using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUS;
using DTO;

namespace SimpleBlogPresentation.Controllers
{
    public class TagController : Controller
    {
        private TagBUS tagBus;
        public TagController()
        {
            tagBus = new TagBUS();
        }
        // GET: Tag
        public ActionResult GetAllTag()
        {
            List<string> error = new List<string>();
            var tags = tagBus.GetAll(error);
            ViewBag.Errors = error;
            return View(tags);
        }
    }
}