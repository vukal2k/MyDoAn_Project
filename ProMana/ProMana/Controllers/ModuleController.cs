using BUS;
using COMMON;
using DTO;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProMana.Controllers
{
    public class ModuleController : Controller
    {
        private ModuleBUS _moduleBus = new ModuleBUS();
        private UserInfoBUS _userInforBus = new UserInfoBUS();
        private ProjectBUS _projectBus = new ProjectBUS();
        private JobRoleBUS _jobRoleBUS = new JobRoleBUS();
        private List<string> errors = new List<string>();
        // GET: Module
        //id is project id
        public async Task<ActionResult> Index(int id)
        {
            var result = await _moduleBus.GetByProject(id, errors);
            ViewBag.IsSuccess = TempData["isSuccess"];
            ViewBag.Project = await _projectBus.GetById(id);
            return View(result);
        }

        // GET: Module/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Module/Create
        public async Task<ActionResult> Create(int id)
        {
            ViewBag.Project = await _projectBus.GetById(id);
            ViewBag.Users = await _projectBus.GetUserNotWatcher(id);
            ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
            return View();
        }

        // POST: Module/Create
        [HttpPost]
        public async Task<ActionResult> Create(Module module, string members)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listMembers = JsonConvert.DeserializeObject<List<MemberParamsViewModel>>(members);
                    var result = await _moduleBus.Insert(module, listMembers, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index","Module",new { id = module.ProjectId});
                    }
                }
                ViewBag.Project = await _projectBus.GetById(module.ProjectId);
                ViewBag.Users = await _projectBus.GetUserNotWatcher(module.ProjectId);
                ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
                ViewBag.InsertFailed = true;
                return View();
            }
            catch
            {
                ViewBag.Project = await _projectBus.GetById(module.ProjectId);
                ViewBag.Users = await _projectBus.GetUserNotWatcher(module.ProjectId);
                ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
                ViewBag.InsertFailed = true;
                return View();
            }
        }

        // GET: Module/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var module = await _moduleBus.GetById(id);
            ViewBag.Users = await _projectBus.GetUserNotWatcher(module.ProjectId);
            ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
            ViewBag.Members = JsonConvert.SerializeObject(module.GetMemberParams());
            return View(module);
        }

        // POST: Module/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Module module, string members)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var listMembers = JsonConvert.DeserializeObject<List<MemberParamsViewModel>>(members);
                    var result = await _moduleBus.Update(module, listMembers, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "Module", new { id = module.ProjectId});
                    }
                }

                _moduleBus = new ModuleBUS();
                var originalModule = await _moduleBus.GetById(module.Id);
                ViewBag.Users = await _projectBus.GetUserNotWatcher(module.ProjectId);
                ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
                ViewBag.InsertFailed = true;
                ViewBag.Members = JsonConvert.SerializeObject(originalModule.GetMemberParams());
                return View(originalModule);
            }
            catch
            {
                _moduleBus = new ModuleBUS();
                var originalModule = await _moduleBus.GetById(module.Id);
                ViewBag.Users = await _projectBus.GetUserNotWatcher(module.ProjectId);
                ViewBag.GetSoftRole = await _jobRoleBUS.GetSoftRole();
                ViewBag.InsertFailed = true;
                ViewBag.Members = JsonConvert.SerializeObject(originalModule.GetMemberParams());
                return View(originalModule);
            }
        }

        // GET: Module/Delete/5
        public async Task<ActionResult> Delete(int moduleId, int projectId)
        {
            var moduleDeleted = await _moduleBus.Delete(moduleId, errors);
            if (moduleDeleted != null)
            {
                TempData["isSuccess"] = true;
                return RedirectToAction("Index", "Module", new { id = moduleDeleted.ProjectId });
            }
            else
            {
                return RedirectToAction("Index", "Module", new { id = projectId });
            }
        }
        
        public async Task<ActionResult> AddMember(string username, int roleId)
        {
            try
            {
                var result = await _userInforBus.GetById(username);
                ViewBag.JobRole = await _jobRoleBUS.GetById(roleId);
                return View(result);
            }
            catch
            {
                return Content("0");
            }
        }

        public async Task<ActionResult> GetMemberOption(int id)
        {
            try
            {
                var result = await _moduleBus.GetMembers(id,User.Identity.GetUserName(),errors);
                return View(result);
            }
            catch
            {
                return Content("0");
            }
        }
    }
}
