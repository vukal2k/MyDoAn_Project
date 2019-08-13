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
    public class ProjectAdminController : Controller
    {
        private ProjectBUS _projectBus = new ProjectBUS();
        private UserInfoBUS _userInfoBus = new UserInfoBUS();
        private List<string> errors = new List<string>();
        // GET: Admin/Project
        public async Task<ActionResult> Index()
        {
            var result = await _projectBus.GetAll();
            ViewBag.IsSuccess = TempData["isSuccess"];
            return View(result);
        }

        // GET: Admin/Project/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Users = await _userInfoBus.GetAll();
            return View();
        }

        // POST: Admin/Project/Create
        [HttpPost]
        public async Task<ActionResult> Create(Project project)
        {
            try
            {
                bool result = false;
                if (ModelState.IsValid)
                {
                    result = await _projectBus.Insert(project, errors);
                }

                if (result)
                {
                    TempData["isSuccess"]=true;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Users = await _userInfoBus.GetAll();
                    ViewBag.Errors = errors;
                    return View(project);
                }
            }
            catch
            {
                ViewBag.Users = await _userInfoBus.GetAll();
                return View();
            }
        }

        // GET: Admin/Project/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.Users = await _userInfoBus.GetAll();
            var result = await _projectBus.GetById(id);
            return View(result);
        }

        // POST: Admin/Project/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Project project)
        {
            try
            {
                bool result = false;
                if (ModelState.IsValid)
                {
                    result = await _projectBus.Update(project, errors);
                }

                if (result)
                {
                    TempData["isSuccess"] = true;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Users = await _userInfoBus.GetAll();
                    ViewBag.Errors = errors;
                    return View(project);
                }
            }
            catch
            {
                ViewBag.Users = await _userInfoBus.GetAll();
                return View();
            }
        }

        // GET: Admin/Project/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _projectBus.Delete(id, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "Project");
                    }
                }
            }
            catch
            {
            }
            return RedirectToAction("Index", "Project");
        }
    }
}

