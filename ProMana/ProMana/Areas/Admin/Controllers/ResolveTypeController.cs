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
    public class ResolveTypeController : Controller
    {
        private ResolveTypeBUS _resolveType = new ResolveTypeBUS();
        private List<string> errors = new List<string>();
        // GET: Admin/ReolveType
        public async Task<ActionResult> Index()
        {
            var result = await _resolveType.GetAll();
            ViewBag.IsSuccess = TempData["isSuccess"];
            return View(result);
        }

        // GET: Admin/ReolveType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ReolveType/Create
        [HttpPost]
        public async Task<ActionResult> Create(ResolveType resolveType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _resolveType.Insert(resolveType, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "ResolveType");
                    }
                }
            }
            catch
            {
            }
            ViewBag.InsertFailed = true;
            return View();
        }

        // GET: Admin/ReolveType/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var result = await _resolveType.GetById(id);
            return View(result);
        }

        // POST: Admin/ReolveType/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(ResolveType resolveType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _resolveType.Update(resolveType, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "ResolveType");
                    }
                }
            }
            catch
            {
            }
            ViewBag.InsertFailed = true;
            return View(resolveType);
        }

        // GET: Admin/ResolveType/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _resolveType.Delete(id, errors);
                    if (result)
                    {
                        TempData["isSuccess"] = true;
                        return RedirectToAction("Index", "ResolveType");
                    }
                }
            }
            catch
            {
            }
            return RedirectToAction("Index", "ResolveType");
        }
    }
}