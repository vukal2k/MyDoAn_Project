using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProMana.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class JobRoleController : Controller
    {
        private JobRoleBUS _jobRoleBUS = new JobRoleBUS();
        private List<string> errors = new List<string>();
        // GET: Admin/JobRole
        public async Task<ActionResult> Index()
        {
            var result = await _jobRoleBUS.GetAll();
            ViewBag.IsSuccess = TempData["isSuccess"];
            return View(result);
        }

        // GET: Admin/JobRole/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/JobRole/Create
        [HttpPost]
        public async Task<ActionResult> Create(JobRole jobRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _jobRoleBUS.Insert(jobRole,errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "JobRole");
                    }
                }
            }
            catch
            {
            }
            ViewBag.InsertFailed = true;
            return View();
        }

        // GET: Admin/JobRole/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var result = await _jobRoleBUS.GetById(id);
            return View(result);
        }

        // POST: Admin/JobRole/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(JobRole jobRole)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _jobRoleBUS.Update(jobRole, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "JobRole");
                    }
                }
            }
            catch
            {
            }
            ViewBag.InsertFailed = true;
            return View(jobRole);
        }

        // GET: Admin/JobRole/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _jobRoleBUS.Delete(id, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "JobRole");
                    }
                }
            }
            catch
            {
            }
            return RedirectToAction("Index", "JobRole");
        }
    }
}
