using BUS;
using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProMana.Controllers
{
    public class ModuleController : Controller
    {
        private ModuleBUS _moduleBus = new ModuleBUS();
        private ProjectBUS _projectBus = new ProjectBUS();
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
            return View();
        }

        // POST: Module/Create
        [HttpPost]
        public async Task<ActionResult> Create(Module module)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _moduleBus.Insert(module,errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index","Module",new { id = module.ProjectId});
                    }
                    else
                    {
                        ViewBag.Project = await _projectBus.GetById(module.ProjectId);
                        ViewBag.InsertFailed = true;
                        return View();
                    }
                }
                ViewBag.InsertFailed = true;
                return View();
            }
            catch
            {
                ViewBag.Project = await _projectBus.GetById(module.ProjectId);
                ViewBag.InsertFailed = true;
                return View();
            }
        }

        // GET: Module/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var module = await _moduleBus.GetById(id);
            return View(module);
        }

        // POST: Module/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Module module)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _moduleBus.Update(module, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "Module", new { id = module.ProjectId});
                    }
                    else
                    {
                        ViewBag.Project = await _projectBus.GetById(module.ProjectId);
                        ViewBag.InsertFailed = true;
                        return View();
                    }
                }
                ViewBag.InsertFailed = true;
                return View();
            }
            catch
            {
                ViewBag.Project = await _projectBus.GetById(module.ProjectId);
                ViewBag.InsertFailed = true;
                return View();
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
    }
}
