using BUS;
using DTO;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserInfoBUS _userBus = new UserInfoBUS();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            var roleStore = new RoleStore<IdentityRole>();
            //var roleManage = new RoleManager<IdentityRole>(roleStore);
            //var roles = roleManage.Roles.ToList();
            //ViewBag.Roles = roles;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(UserInfoAccoutViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                RegisterViewModel userViewModel = new RegisterViewModel
                {
                    Username = viewModel.Username,
                    ConfirmPassword = viewModel.ConfirmPassword,
                    Email = viewModel.Email,
                    Password = viewModel.Password
                };
                UserInfo userInfo = new UserInfo
                {
                    Company = viewModel.Company,
                    CountExperience = viewModel.CountExperience,
                    CurrentJob = viewModel.CurrentJob,
                    FullName = viewModel.FullName,
                    TimeUnit = viewModel.TimeUnit,
                    UserName = viewModel.Username,
                    IsActive = true,
                    Email = viewModel.Email
                };

                var user = new ApplicationUser { UserName = viewModel.Username, Email = viewModel.Email };
                var result = await UserManager.CreateAsync(user, viewModel.Password);
 
                if (result.Succeeded)
                {
                    //insert role
                    var roleManager = HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
                    //var role = roleManager.Roles.Where(r => r.Name.Equals("Normal User")).FirstOrDefault();
                    var x = await UserManager.AddToRoleAsync(user.Id, "Normal User");

                    if (x.Succeeded)
                    {
                        //insert user info
                        var resultUserInfo = await _userBus.Create(userInfo, new List<string>());
                        if (!resultUserInfo)
                        {
                            var userDelete = await UserManager.FindByEmailAsync(viewModel.Email);
                            if (userDelete != null)
                            {
                                await UserManager.DeleteAsync(userDelete);
                            }

                            return Json(new { Status = true, Message = "Resgister failed!" });
                        }
                        else
                        {
                            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            var callbackUrl = Url.Action(
                               "ConfirmEmail", "Account",
                               new { userId = user.Id, code = code },
                               protocol: Request.Url.Scheme);

                            var emailContent = "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>";
                            // ViewBag.Link = callbackUrl;   // Used only for initial demo.

                            SendMail(new List<string>(), viewModel.Email, "[PROMANA] Please confirm your email!", emailContent);
                            return View("DisplayEmail");
                        }
                    }
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Users/Edit/1
        [HttpGet]
        public async Task<ActionResult> UserProfile(List<string> errors = null)
        {
            var id = User.Identity.GetUserName();
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
            ViewBag.Errors = errors;
            ViewBag.isSuccess = TempData["isSuccess"];
            return View(new EditUserAccountViewModel()
            {
                Id = user.Id,
                Company = userInfo.Company,
                CountExperience = userInfo.CountExperience,
                CurrentJob = userInfo.CurrentJob,
                Email = user.Email,
                FullName = userInfo.FullName,
                TimeUnit = userInfo.TimeUnit,
                Username = user.UserName,

                RolesList = userRoles
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UserProfile(EditUserAccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                List<string> listError = new List<string>();
                EditUserViewModel editUser = new EditUserViewModel
                {
                    Id = viewModel.Id,
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
                    return RedirectToAction("UserProfile", "Account", new {errors = listError.First() });
                }

                user.Email = editUser.Email;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                var result = await UserManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return RedirectToAction("UserProfile", "Account", new { errors = listError.First() });
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
                        Email = user.Email
                    };
                    var resultUserInfo = await _userBus.Update(userInfo, new List<string>());
                }
                TempData["isSuccess"] = true;
                return RedirectToAction("UserProfile", "Account");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var x = UserManager.FindByEmail(model.Email);
            var y = UserManager.FindByName(model.Username);
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Username);
                //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                if (x != null && y != null)
                {
                    if (x.Id != y.Id)
                    {
                        TempData["Error"] = "Username and E-mail do not match";
                        return View();
                    }
                    else
                    {
                        var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                        var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                        SendMail(new List<string>(), model.Email, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                        ViewBag.Link = callbackUrl;
                        return View("ForgotPasswordConfirmation");
                    }
                }
                TempData["Error"] = "Username and E-mail do not match";
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [NonAction]
        public void SendMail(List<string> error, string emailTo, string subject, string body, string emailCc = "")
        {
            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["host"],
                Port = int.Parse(ConfigurationManager.AppSettings["port"]),
                EnableSsl = bool.Parse(ConfigurationManager.AppSettings["enableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailFrom"], ConfigurationManager.AppSettings["password"])
            };

            using (var smtpMessage = new MailMessage(ConfigurationManager.AppSettings["username"], emailTo))
            {
                smtpMessage.Body = body;
                smtpMessage.IsBodyHtml = true;
                smtpMessage.Subject = subject;
                if (!new MailAddress(emailTo).Address.Equals(emailTo))
                {
                    error.Add("Email destination is not exists");
                }
                if (string.IsNullOrEmpty(emailCc) == false)
                {
                    smtpMessage.CC.Add(new MailAddress(emailCc));
                }
                smtp.Send(smtpMessage);
            }
        }
        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}