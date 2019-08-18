using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BUS;
using DTO;
using System.Web.Mvc;
using SimpleBlogPresentation.Commond;

namespace SimpleBlogPresentation.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryManagerController : Controller
    {
        private CategoryBUS categoryBus;
        public CategoryManagerController()
        {
            categoryBus = new CategoryBUS();
        }
        // GET: Admin/CategoryManager
        public ActionResult Index()
        {
            List<string> error = new List<string>();
            var categories = categoryBus.GetAll(error);
            ViewBag.Errors = error;
            return View(categories);
        }

        // GET: Admin/CategoryManager/Create
        // GET: Admin/Tag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            List<string> error = new List<string>();
            categoryBus.Insert(category, error);
            ViewBag.Errors = error;
            if (error.Count == 0)
            {
                return RedirectToAction("Index", "CategoryManager");
            }
            return View(category);
        }

        // GET: Admin/Tag/Edit/5
        public ActionResult Edit(int id)
        {
            List<string> error = new List<string>();
            Category category = categoryBus.GetById(id, error);
            ViewBag.Errors = error;
            return View(category);
        }

        // POST: Admin/Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            List<string> error = new List<string>();
            categoryBus.Update(category, error);
            ViewBag.Errors = error;
            if (error.Count == 0)
            {
                return RedirectToAction("Index", "CategoryManager");
            }
            return View(category);
        }

        // GET: Admin/Tag/Delete/5
        public ActionResult Delete(int id)
        {
            List<string> error = new List<string>();
            categoryBus.Delete(id, error);
            ViewBag.Errors = error;
            return RedirectToAction("Index", "CategoryManager");
        }
    }
}
