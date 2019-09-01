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
using System.IO;

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
        public async Task<ActionResult> TaskList(int id, bool createTask = false,int chooseTaskId = 0, string filter= "All")
        {
            var project = await _projectBus.GetTaskList(id,filter,User.Identity.GetUserName());
            ViewBag.IsCreateTask = createTask;
            ViewBag.TaskTypes = await _taskTypeBUS.GetAll();
            ViewBag.ChoosedTask = chooseTaskId;
            ViewBag.Filter = filter;
            return View(project);
        }
        

        [HttpGet]
        public async Task<ActionResult> RequestList(int id, bool createTask = false, int chooseTaskId = 0,string filter = "All")
        {
            var project = await _projectBus.GetTaskList(id, filter, User.Identity.GetUserName());
            ViewBag.IsCreateTask = createTask;
            ViewBag.TaskTypes = await _taskTypeBUS.GetAll();
            ViewBag.ChoosedTask = chooseTaskId;
            ViewBag.Filter = filter;
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
            ViewBag.PieChartData = JsonConvert.SerializeObject(result.TaskStatisticByModules, Formatting.None,
                                                                new JsonSerializerSettings
                                                                {
                                                                    PreserveReferencesHandling = PreserveReferencesHandling.None
                                                                });
            ViewBag.PieChartRequestData = JsonConvert.SerializeObject(result.RequestStatisticByModules, Formatting.None,
                                                                new JsonSerializerSettings
                                                                {
                                                                    PreserveReferencesHandling = PreserveReferencesHandling.None
                                                                });
            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult> CloseOpen(int id,int status)
        {
            var project = await _projectBus.GetById(id);
            project.StatusId = status;
            await _projectBus.Update(project,errors);

            return RedirectToAction("Infomation", "Project", new { id = id });
        }

        [HttpGet]
        public async Task<ActionResult> DownloadLog(int id)
        {
            var project = await _projectBus.GetById(id);
            // Gọi lại hàm để tạo file excel
            System.IO.Stream stream = await _projectBus.DownloadLog(id, errors);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", $"attachment; filename={project.Code}-ProjectLog.xlsx");
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
            return Content("Success");
        }
    }
}
