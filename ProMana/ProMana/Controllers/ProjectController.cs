using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUS;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProMana.Controllers
{
    public class ProjectController : Controller
    {
        private ProjectBUS projectBus = new ProjectBUS();
        private JobRoleBUS JobRoleBUS = new JobRoleBUS();
        List<string> errors = new List<string>();
        // GET: Project
        public ActionResult Index()
        {
            return View();
        }
        

        // GET: Project/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.GetUserDoNotInProject = await projectBus.GetUserDoNotInProject(0);
            ViewBag.GetSoftRole = await JobRoleBUS.GetSoftRole();
            return View();
        }

        // POST: Project/Create
        [HttpPost]
        public async Task<ActionResult> Create(Project project,string members)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listMembers = JsonConvert.DeserializeObject<List<MemberParamsViewModel>>(members);
                    await projectBus.Create(project, listMembers, "pmtest",errors);
                }

                ViewBag.Errors = errors;

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<ActionResult> KanbanBoard(int id)
        {
            var project = await projectBus.GetById(id);
            return View(project);
        }

        //[HttpGet]
        //public async Task<ActionResult> KanbanBoardFilter(int projectId, int moduleId)
        //{
        //    ViewBag.ModuleId = moduleId;
        //    var project = await projectBus.GetById(projectId);
        //    return View();
        //}

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Project/Edit/5
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

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
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
