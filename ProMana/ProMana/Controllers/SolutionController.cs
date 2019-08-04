﻿using BUS;
using DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ProMana.Controllers
{
    public class SolutionController : Controller
    {
        private TaskBUS _taskBus = new TaskBUS();
        private SolutionBUS _solutionBUS = new SolutionBUS();
        private ResolveTypeBUS _resolveTypeBUS = new ResolveTypeBUS();
        private List<string> errors = new List<string>();
        // GET: Solution
        public ActionResult Index()
        {
            return View();
        }

        // GET: Solution/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Solution/Create
        public async Task<ActionResult> Create(int id)
        {
            ViewBag.Task = await _taskBus.GetById(id,errors);
            ViewBag.ResolveType = await _resolveTypeBUS.GetAll();
            ViewBag.Errors = errors;
            return View();
        }

        // POST: Solution/Create
        [HttpPost]
        public async Task<ActionResult> CreateSoution(string solutionJson)
        {
            try
            {
                Solutionn solution = JsonConvert.DeserializeObject<Solutionn>(solutionJson);
                bool result = false;

                result = await _solutionBUS.Create(solution, "pmtest", errors);

                if (result)
                {
                    return Content("1");
                }
                else
                {
                    return Content(JsonConvert.SerializeObject(errors));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Solution/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Solution/Edit/5
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

        // GET: Solution/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Solution/Delete/5
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