using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTO;
using BUS;

namespace SimpleBlogPresentation.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryBUS categoryBus;
        public CategoryController()
        {
            categoryBus = new CategoryBUS();
        }
        // GET: Category
        public ActionResult GetCategories()
        {
            PostBUS postBus = new PostBUS();
            List<string> error = new List<string>();
            var categories = categoryBus.GetAll(error);
            var test = postBus.GetAllPostPublishedByPage(error);
            ViewBag.Errors = error;
            return View(categories);
        }

        public ActionResult LoadNotFromIndex()
        {
            TempData["ActionName"]= "GetPostByCate";
            return RedirectToAction("Index","Home");
        }
    }
}