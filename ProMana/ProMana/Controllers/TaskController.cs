using BUS;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProMana.Controllers
{
    public class TaskController : Controller
    {
        private TaskBUS _taskBus = new TaskBUS();
        private ProjectBUS _projectBus = new ProjectBUS();
        private TaskTypeBUS _taskTypeBUS = new TaskTypeBUS();
        private List<string> errors = new List<string>();
        // GET: Task
        public ActionResult Index()
        {
            return View();
        }

        // GET: Task/Details/5
        public async Task<ActionResult> Details(int id, bool isSummary=false)
        {
            var task = await _taskBus.GetById(id,errors);
            if (isSummary)
            {
                return PartialView("~/Views/Task/DetailSummary.cshtml", task);
            }
            return View(task);
        }

        // GET: Task/Details/5
        public async Task<ActionResult> DetailsRequest(int id, bool isSummary = false)
        {
            var task = await _taskBus.GetById(id, errors);
            if (isSummary)
            {
                return PartialView("~/Views/Task/RequestDetailSummary.cshtml", task);
            }
            return View(task);
        }

        // GET: Task/Details/5
        public async Task<ActionResult> ChangeStatus(int taskId, int statusId)
        {
            var result = await _taskBus.ChangeStatus(taskId, statusId,User.Identity.GetUserName());
            if (result)
            {
                return Content("1");
            }
            return Content("0");
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        public async Task<ActionResult> Create(DTO.Task task , int projectId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskBus.Insert(task,User.Identity.GetUserName(),errors);
                    if (result)
                    {
                        var redirectTo = "window.location.href = '" + Url.Action("TaskList", "Project", new { id = projectId, chooseTaskId = task.Id }) + "';";
                        return JavaScript(redirectTo);
                    }
                }

                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
            catch
            {
                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
        }

        // GET: Task/Create
        public ActionResult CreateRequest()
        {
            return View();
        }

        // POST: Task/Create
        [HttpPost]
        public async Task<ActionResult> CreateRequest(DTO.Task task, int projectId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskBus.InsertRequest(task, User.Identity.GetUserName(), errors);
                    if (result)
                    {
                        var redirectTo = "window.location.href = '" + Url.Action("RequestList", "Project", new { id = projectId, chooseTaskId = task.Id }) + "';";
                        return JavaScript(redirectTo);
                    }
                }

                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
            catch
            {
                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
        }

        // GET: Task/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var task = await _taskBus.GetById(id, errors);
            ViewBag.Project = await _projectBus.GetById(task.Module.ProjectId);
            ViewBag.TaskTypes = await _taskTypeBUS.GetAll();
            return View(task);
        }

        // POST: Task/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(DTO.Task task, bool isCancel = false)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskBus.Update(task, errors);
                    if (result!=null)
                    {
                        return PartialView("~/Views/Task/Details.cshtml", result);
                        //return JavaScript("location.reload(true)");
                        
                    }
                }

                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
            catch
            {
                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
        }

        // GET: Task/Edit/5
        public async Task<ActionResult> ConvertToTask(int id)
        {
            var task = await _taskBus.GetById(id, errors);
            ViewBag.Project = await _projectBus.GetById(task.Module.ProjectId);
            ViewBag.TaskTypes = await _taskTypeBUS.GetAll();
            return View(task);
        }

        // POST: Task/Edit/5
        [HttpPost]
        public async Task<ActionResult> ConvertToTask(DTO.Task task, bool isCancel = false)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskBus.ConvertToTask(task, errors,User.Identity.GetUserName());
                    if (result != null)
                    {
                        var redirectTo = "window.location.href = '" + Url.Action("TaskList", "Project", new { id = result.Module.ProjectId, chooseTaskId = result.Id }) + "';";
                        return JavaScript(redirectTo);

                    }
                }

                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
            catch
            {
                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
        }

        // GET: Task/Edit/5
        public async Task<ActionResult> EditRequest(int id)
        {
            var task = await _taskBus.GetById(id, errors);
            ViewBag.Project = await _projectBus.GetById(task.Module.ProjectId);
            ViewBag.TaskTypes = await _taskTypeBUS.GetAll();
            return View(task);
        }

        // POST: Task/Edit/5
        [HttpPost]
        public async Task<ActionResult> EditRequest(DTO.Task task, bool isCancel = false)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _taskBus.UpdateRequest(task, errors,User.Identity.GetUserName());
                    if (result != null)
                    {
                        return PartialView("~/Views/Task/DetailsRequest.cshtml", result);
                        //return JavaScript("location.reload(true)");

                    }
                }

                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
            catch
            {
                return PartialView("~/Views/Shared/_ActionFailed.cshtml");
            }
        }
        

        // GET: Task/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Task/Delete/5
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
