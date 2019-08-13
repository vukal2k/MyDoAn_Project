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
        private ProjectBUS _projectBus = new ProjectBUS();
        private JobRoleBUS _jobRoleBUS = new JobRoleBUS();
        private TaskTypeBUS _taskTypeBUS = new TaskTypeBUS();
        List<string> errors = new List<string>();
        // GET: Project
        public ActionResult Index()
        {
            return View();
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
                    result=await _projectBus.Create(project, listMembers, "pmtest",errors);
                }

                if (result)
                {
                    return RedirectToAction("Index");
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
        public async Task<ActionResult> Infomation(int id)
        {
            var project = await _projectBus.GetById(id);
            ViewBag.GetUserDoNotInProject = await _projectBus.GetUserDoNotInProject(0);
            ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
            ViewBag.GetAllRole = await _jobRoleBUS.GetAll();
            var members = project.RoleInProjects.Select(r => new MemberDetailViewModel
            {
                RoleId = r.RoleId,
                FullName = r.UserInfo.FullName,
                RoleTitle = r.JobRole.Title,
                Username = r.UserName
            });
            var memberJson = JsonConvert.SerializeObject(members.ToList());
            ViewBag.GetMemberInProject = memberJson;
            return View(project);
        }

        [HttpPost]
        public async Task<ActionResult> Infomation(Project project, string members)
        {
            try
            {
                Project result = null;
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
    }
}
