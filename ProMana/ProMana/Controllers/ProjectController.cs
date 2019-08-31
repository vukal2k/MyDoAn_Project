using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BUS;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using COMMON;

namespace ProMana.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private ProjectBUS _projectBus = new ProjectBUS();
        private JobRoleBUS _jobRoleBUS = new JobRoleBUS();
        private TaskTypeBUS _taskTypeBUS = new TaskTypeBUS();
        private UserInfoBUS _userInfoBUS = new UserInfoBUS();
        List<string> errors = new List<string>();
        // GET: Project
        public async Task<ActionResult> Index()
        {
            var result = await _projectBus.GetMyProject(User.Identity.GetUserName());
            return View(result);
        }

        public async Task<ActionResult> Details(int id)
        {
            var result = await _projectBus.GetById(id);
            return View(result);
        }

        // GET: Project/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.GetUserDoNotInProject = await _projectBus.GetUserDoNotInProject(0);
            ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
            return View();
        }
        // POST: Project/Create
        [HttpPost]
        public async Task<ActionResult> Create(Project project,string members)
        {
            try
            {
                bool result = false;
                if (ModelState.IsValid)
                {
                    var listMembers = JsonConvert.DeserializeObject<List<MemberParamsViewModel>>(members);
                    result=await _projectBus.Create(project, listMembers, User.Identity.GetUserName(),errors);
                }

                if (result)
                {
                    return RedirectToAction("Index","Module",new { id = project.Id});
                }
                else
                {
                    ViewBag.GetUserDoNotInProject = await _projectBus.GetUserDoNotInProject(0);
                    ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
                    ViewBag.Errors = errors;
                    return View(project);
                }
            }
            catch
            {
                ViewBag.GetUserDoNotInProject = await _projectBus.GetUserDoNotInProject(0);
                ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
                return View(project);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GanttChart(int id, int page=0)
        {
            var ganttChart = await _projectBus.GetGanttChart(id,page);
            return View(ganttChart);
        }

        [HttpGet]
        public async Task<ActionResult> KanbanBoard(int id)
        {
            var project = await _projectBus.GetById(id);
            return View(project);
        }

        [HttpGet]
        public async Task<ActionResult> TaskList(int id, bool createTask = false,int chooseTaskId = 0)
        {
            var project = await _projectBus.GetById(id);
            ViewBag.IsCreateTask = createTask;
            ViewBag.TaskTypes = await _taskTypeBUS.GetAll();
            ViewBag.ChoosedTask = chooseTaskId;
            return View(project);
        }

        [HttpGet]
        public async Task<ActionResult> RequestList(int id, bool createTask = false, int chooseTaskId = 0)
        {
            var project = await _projectBus.GetById(id);
            ViewBag.IsCreateTask = createTask;
            ViewBag.TaskTypes = await _taskTypeBUS.GetAll();
            ViewBag.ChoosedTask = chooseTaskId;
            return View(project);
        }

        [HttpGet]
        public async Task<ActionResult> Infomation(int id)
        {
            var project = await _projectBus.GetById(id);
            ViewBag.GetUserDoNotInProject = await _projectBus.GetUserDoNotInProject(id);
            ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
            ViewBag.GetAllRole = await _jobRoleBUS.GetAll();

            var module = project.Modules.Where(m => m.Title.Equals(COMMON.HardFixJobRoleTitle.Watcher)).FirstOrDefault();
            ViewBag.WatchersJson = JsonConvert.SerializeObject(module.GetMemberParams());
           
            return View(project);
        }

        [HttpPost]
        public async Task<ActionResult> Infomation(Project project, string members)
        {
            try
            {
                bool result = false;
                if (ModelState.IsValid)
                {
                    var listMembers = JsonConvert.DeserializeObject<List<MemberParamsViewModel>>(members);
                    result = await _projectBus.Update(project, listMembers, errors);
                }

                ViewBag.GetUserDoNotInProject = await _projectBus.GetUserDoNotInProject(0);
                ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
                ViewBag.GetAllRole = await _jobRoleBUS.GetAll();
                ViewBag.Errors = errors;
                ViewBag.isSuccess = !errors.Any();
                return RedirectToAction("Infomation", "Project", new { Id = project.Id });
            }
            catch
            {
                ViewBag.GetUserDoNotInProject = await _projectBus.GetUserDoNotInProject(0);
                ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
                return RedirectToAction("Infomation","Project",new { Id =project.Id});
            }
        }

        [HttpGet]
        public async Task<ActionResult> AddWatcher(string username)
        {
            try
            {
                var result = await _userInfoBUS.GetById(username);
               return View(result);
            }
            catch
            {
                return Content("0");
            }
        }

        [HttpGet]
        public async Task<ActionResult> Statistic(int id)
        {
            var result = await _projectBus.Statistical(id);
            return View(result);
        }
    }
}
