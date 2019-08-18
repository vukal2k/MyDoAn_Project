using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BUS;
using DTO;
using log4net;
using System.IO;

namespace SimpleBlogPresentation.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TagManagerController : Controller
    {
        private TagBUS tagBus = new TagBUS();
        private static readonly ILog log = LogManager.GetLogger(typeof(TagManagerController).Name);

        // GET: Admin/Tag
        public ActionResult Index()
        {
            List<string> error = new List<string>();

            var tags = tagBus.GetAll(error);
            ViewBag.Errors = error;
            foreach (var item in error)
            {
                log.Error($"Error is {item}");
            }

            return View(tags);
        }

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
        public ActionResult Create([Bind(Include = "TagId,Name,UrlSlug,Description")] Tag tag)
        {
            List<string> error = new List<string>();
            tagBus.Insert(tag, error);
            ViewBag.Errors = error;
            if (error.Count == 0)
            {
                return RedirectToAction("Index", "TagManager");
            }
            return View(tag);
        }

        // GET: Admin/Tag/Edit/5
        public ActionResult Edit(int id)
        {
            List<string> error = new List<string>();
            Tag tag = tagBus.GetById(id,error);
            ViewBag.Errors = error;
            return View(tag);
        }

        // POST: Admin/Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TagId,Name,UrlSlug,Description")] Tag tag)
        {
            List<string> error = new List<string>();
            tagBus.Update(tag, error);
            ViewBag.Errors = error;
            if (error.Count == 0)
            {
                return RedirectToAction("Index", "TagManager");
            }
            return View(tag);
        }

        // GET: Admin/Tag/Delete/5
        public ActionResult Delete(int id)
        {
            List<string> error = new List<string>();
            tagBus.Delete(id, error);
            ViewBag.Errors = error;
            return RedirectToAction("Index", "TagManager");
        }
    }
}
