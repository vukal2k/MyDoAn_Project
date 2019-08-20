using BUS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProMana.Areas.Admin.Controllers
{
    public class AccountAdminController : Controller
    {
        private UserInfoBUS _userBus = new UserInfoBUS();
        // GET: Admin/AccountAdmin
        public async Task<ActionResult> Index()
        {
            return View(await _userBus.GetAll());
        }

        // GET: Admin/AccountAdmin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/AccountAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AccountAdmin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/AccountAdmin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/AccountAdmin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/AccountAdmin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/AccountAdmin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
