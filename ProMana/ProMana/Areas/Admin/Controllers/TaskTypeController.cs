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
    public class TaskTypeController : Controller
    {
        private TaskTypeBUS _taskTytpeBus = new TaskTypeBUS();
        private List<string> errors = new List<string>();
        // GET: Admin/TaskType
        public async Task<ActionResult> Index()
        {
            var result = await _taskTytpeBus.GetAll();
            ViewBag.IsSuccess = TempData["isSuccess"];
            return View(result);
        }

        // GET: Admin/TaskType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TaskType/Create
        [HttpPost]
        public async Task<ActionResult> Create(TaskType taskType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskTytpeBus.Insert(taskType, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "TaskType");
                    }
                }
            }
            catch
            {
            }
            ViewBag.InsertFailed = true;
            return View();
        }

        // GET: Admin/TaskType/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var result = await _taskTytpeBus.GetById(id);
            return View(result);
        }

        // POST: Admin/TaskType/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(TaskType taskType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskTytpeBus.Update(taskType, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "TaskType");
                    }
                }
            }
            catch
            {
            }
            ViewBag.InsertFailed = true;
            return View(taskType);
        }

        // GET: Admin/TaskType/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskTytpeBus.Delete(id, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "TaskType");
                    }
                }
            }
            catch
            {
            }
            return RedirectToAction("Index", "TaskType");
        }
    }
}
