using BUS;
using DTO;
using IdentitySample.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProMana.Areas.Admin.Controllers
{
    public class AccountAdminController : Controller
    {
        private UserInfoBUS _userBus = new UserInfoBUS();
        // GET: Admin/AccountAdmin
        public async Task<ActionResult> Index()
        {
            ViewBag.IsSuccess = TempData["isSuccess"];
            return View(await _userBus.GetAll());
        }

        public AccountAdminController()
        {
        }

        public AccountAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        
        //
        // GET: /Users/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserInfoAccoutViewModel viewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                RegisterViewModel userViewModel=new RegisterViewModel {
                    Username=viewModel.Username,
                    ConfirmPassword=viewModel.ConfirmPassword,
                    Email=viewModel.Email,
                    Password=viewModel.Password
                };
                UserInfo userInfo = new UserInfo
                {
                    Company=viewModel.Company,
                    CountExperience=viewModel.CountExperience,
                    CurrentJob=viewModel.CurrentJob,
                    FullName=viewModel.FullName,
                    TimeUnit=viewModel.TimeUnit,
                    UserName=viewModel.Username,
                    IsActive=true,
                    Email=viewModel.Email
                };

                var user = new ApplicationUser { UserName = userViewModel.Username, Email = userViewModel.Email ,EmailConfirmed=true};
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                        else
                        {
                            //insert user info
                            var resultUserInfo = await _userBus.Create(userInfo, new List<string>());
                            if (!resultUserInfo)
                            {
                                var userDelete = await UserManager.FindByEmailAsync(viewModel.Email);
                                if (userDelete!=null)
                                {
                                    await UserManager.DeleteAsync(userDelete);
                                }
                                ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                                return View();
                            }
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                TempData["isSuccess"] = true;
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        [HttpGet]
        public async Task<ActionResult> Edit(string id, List<string>errors = null)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //get account info
            var user = await UserManager.FindByNameAsync(id);

            //get user info
            var userInfo = await _userBus.GetById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            ViewBag.Errors = errors;
            return View(new EditUserAccountViewModel()
            {
                Id = user.Id,
                Company = userInfo.Company,
                CountExperience = userInfo.CountExperience,
                CurrentJob = userInfo.CurrentJob,
                Email = user.Email,
                FullName = userInfo.FullName,
                TimeUnit = userInfo.TimeUnit,
                Username=user.UserName,

                RolesList= userRoles
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserAccountViewModel viewModel, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                List<string> listError = new List<string>();
                EditUserViewModel editUser = new EditUserViewModel {
                    Id=viewModel.Id,
                    Email = viewModel.Email
                };
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var findWithEmail = await UserManager.FindByEmailAsync(viewModel.Email);
                if (findWithEmail != null && !findWithEmail.UserName.Equals(user.UserName))
                {
                    listError.Add("Email is exists");
                    return RedirectToAction("Edit", "AccountAdmin", new { id = user.UserName, errors = listError.First() });
                }

               user.Email = editUser.Email;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.UpdateAsync(user);
                result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return RedirectToAction("Edit","AccountAdmin", new {id=user.UserName, errors = listError });
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return RedirectToAction("Edit", "AccountAdmin", new { id = user.UserName, errors = listError });
                }
                else //update userinfor
                {
                    UserInfo userInfo = new UserInfo
                    {
                        Company = viewModel.Company,
                        CountExperience = viewModel.CountExperience,
                        CurrentJob = viewModel.CurrentJob,
                        FullName = viewModel.FullName,
                        TimeUnit = viewModel.TimeUnit,
                        UserName = user.UserName,
                        IsActive = true,
                        Email=user.Email
                    };
                    var resultUserInfo = await _userBus.Update(userInfo, new List<string>());
                }
                TempData["isSuccess"] = true;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        [HttpGet]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByNameAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                else
                {
                    await _userBus.Delete(id, new List<string>());
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
