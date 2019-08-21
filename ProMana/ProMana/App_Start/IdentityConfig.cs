using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace IdentitySample.Models
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole,string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public void SendMail(List<string> error, string emailTo, string subject, string body, string emailCc = "")
        {
            try
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
                    if (!new MailAddress(emailTo).Address.Equals(emailTo))
                    {
                        error.Add("Email destination is not exists");
                        return;
                    }
                    if (string.IsNullOrEmpty(emailCc) == false)
                    {
                        smtpMessage.CC.Add(new MailAddress(emailCc));
                    }
                    smtpMessage.Subject = subject;
                    smtpMessage.Body = body;
                    smtpMessage.IsBodyHtml = true;
                    smtp.Send(smtpMessage);
                }
            }
            catch (Exception ex)
            {
                error.Add(ex.Message);
            }
        }

        public Task SendAsync(IdentityMessage message)
        {
            
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string name = "SuperAdmin";
            const string password = "123456";
            const string roleName = "Super User";
            const string roleName2 = "Normal User";
            //HieuPD2
            const string name1 = "pmtest";
            const string password1 = "123456";
            const string email1 = "vuanlbv@gmail.com";
            //TramTLN
            const string name2 = "devtest";
            const string password2 = "123456";
            const string email2 = "vuanlbv1@gmail.com";
            //HungNQ37
            const string name3 = "teamleadtest";
            const string password3 = "123456";
            const string email3 = "vuanlbv2@gmail.com";
            //HuyVD8
            const string name4 = "testertest";
            const string password4 = "123456";
            const string email4 = "vuanlbv3@gmail.com";
            //AnhNK7
            const string name5 = "watchertest";
            const string password5 = "123456";
            const string email5 = "vuanlbv4@gmail.com";

            //Create Role SuperAdmin , NormalAdmin
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new IdentityRole(roleName);
                var roleresult = roleManager.Create(role);
            }
            var role1 = roleManager.FindByName(roleName2);
            if (role1 == null)
            {
                role1 = new IdentityRole(roleName2);
                var roleresult = roleManager.Create(role1);
            }


            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name + "@gmail.com" };
                var result = userManager.Create(user, password);
            }
            var user1 = userManager.FindByName(name1);
            if (user1 == null)
            {
                user1 = new ApplicationUser { UserName = name1, Email = email1 };
                var result = userManager.Create(user1, password1);
            }
            var user2 = userManager.FindByName(name2);
            if (user2 == null)
            {
                user2 = new ApplicationUser { UserName = name2, Email = email2 };
                var result = userManager.Create(user2, password2);
            }
            var user3 = userManager.FindByName(name3);
            if (user3 == null)
            {
                user3 = new ApplicationUser { UserName = name3, Email = email3 };
                var result = userManager.Create(user3, password3);
            }
            var user4 = userManager.FindByName(name4);
            if (user4 == null)
            {
                user4 = new ApplicationUser { UserName = name4, Email = email4 };
                var result = userManager.Create(user4, password4);
            }
            var user5 = userManager.FindByName(name5);
            if (user5 == null)
            {
                user5 = new ApplicationUser { UserName = name5, Email = email5 };
                var result = userManager.Create(user5, password5);
            }

            var user6 = userManager.FindByName("devtest2");
            if (user6 == null)
            {
                user6 = new ApplicationUser { UserName = "devtest2", Email = "devtest2@gmail.com" };
                var result = userManager.Create(user6, "123456");
            }

            var user7 = userManager.FindByName("teamleadtest2");
            if (user7 == null)
            {
                user7 = new ApplicationUser { UserName = "teamleadtest2", Email = "teamleadtest2@gmail.com" };
                var result = userManager.Create(user7, "123456");
            }

            var user8 = userManager.FindByName("tester2");
            if (user8 == null)
            {
                user8 = new ApplicationUser { UserName = "tester2", Email = "tester2@gmail.com" };
                var result = userManager.Create(user8, "123456");
            }

            var user9 = userManager.FindByName("watchertest2");
            if (user9 == null)
            {
                user9 = new ApplicationUser { UserName = "watchertest2", Email = "watchertest2@gmail.com" };
                var result = userManager.Create(user9, "123456");
            }
        }
    }


    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) : 
            base(userManager, authenticationManager) { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}