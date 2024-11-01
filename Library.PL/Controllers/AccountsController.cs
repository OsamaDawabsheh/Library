using AutoMapper;
using Library.DAL.Data;
using Library.DAL.Models;
using Library.PL.Areas.Dashboard.ViewModels;
using Library.PL.Helpers;
using Library.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Library.PL.Controllers
{
    public class AccountsController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountsController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ViewBag.EmailExist = "Email is already taken";

                return View(model);
            }

            model.Img = FilesSettings.UploadFile(model.Image, "users");


            var user = mapper.Map<ApplicationUser>(model);

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "user");

                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmEmailURL = Url.Action("ConfirmEmail", "Accounts", new { id = user.Id, token = token }, protocol: HttpContext.Request.Scheme);

                var body = $@"
<html>
        <body>
 <div style='width: 100%; padding: 20px; display: flex; justify-content: center;'>
            <table style='max-width: 400px; border: 3px solid #4CAF50; padding: 20px; border-radius: 50px; text-align: center;'>
                <tr>
                    <td>
                        <h3 style='color: #4CAF50; font-size: 24px; margin-bottom: 20px;'>Confirm Email</h3>
                    </td>
                </tr>
                <tr>
                    <td style='pading: 10px 0;'>
                        <p style='color: #333; font-size: 16px;'>
                            If you requested to confirm your email, please click the link below.
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style='padding: 20px 0;'>
                        <a href='{confirmEmailURL}'' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 20px; display: inline-block;'>
                            Confirm Email
                        </a>
                    </td>
                </tr>
                <tr>
                    <td style='padding: 10px 0;'>
                        <p style='color: #333; font-size: 14px;'>
                            If you didn't request a email confirm, please ignore this email.
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style='padding-top: 20px;'>
                        <p style='color: #666; font-size: 14px;'>
                            Best regards,<br>Library
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        </body>
    </html>
";

                var email = new Email()
                {
                    Subject = "Confirm Email",
                    Receiver = model.Email,
                    Body = body
                };
                EmailSettings.SendEmail(email);

                return RedirectToAction(nameof(CheckConfirmEmail));
            }
            TempData["Error"] = string.Join(" ", result.Errors.Select(e => e.Description));
            return View(model);
        }

        public IActionResult CheckConfirmEmail()
        {
            return View();
        }

        public async Task<IActionResult> ConfirmEmail(string id , string token)
        {
            if(id is null || token is null)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Register", "Accounts", new { area = "Dashboard" });

                }

                RedirectToAction("Resister","Accounts");
            }
            var user = await userManager.FindByIdAsync(id);

            if (user is not null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                        TempData["ConfirmEmail"] = "You confirmed your email successfully";
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "Accounts", new {area="Dashboard"});

                    }

                    return RedirectToAction(nameof (Login));
                }
            }
            TempData["ConfirmError"] = "Error when trying to confirm email , please try again";

            return RedirectToAction("CheckConfirmEmail","Accounts");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index),"Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var check = await userManager.CheckPasswordAsync(user, model.Password);
                if (check)
                {

                    var result = await signInManager.PasswordSignInAsync(user , model.Password,model.RememberMe,lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        if (User.IsInRole("Admin")) {
                            return RedirectToAction("Index", "Books", new { area = "Dashboard" });

                        }
                        TempData["Success"] = "You login successfuuly";
                        return RedirectToAction("Index","Home");
                    }
                }
            }
            ViewBag.Error = "InCorrect Email or Password";
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            TempData["Logout"] = "You logout successfully";
            await signInManager.SignOutAsync();
            return RedirectToAction("Login","Accounts");
        }

        public IActionResult Profile( string id)
        {
            var user = context.Users.Find(id);

            if (user == null)
            {
                return NotFound(); 
            }

            return View(mapper.Map<UserVM>(user));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ForgotPasswordVM model)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("ForgotPassword", model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null) {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordURL = Url.Action("ChangePassword", "Accounts", new {email= model.Email , token = token},protocol: HttpContext.Request.Scheme);
                var body = $@"
<html>
        <body>
 <div style='width: 100%; padding: 20px; display: flex; justify-content: center;'>
            <table style='max-width: 400px; border: 3px solid #4CAF50; padding: 20px; border-radius: 50px; text-align: center;'>
                <tr>
                    <td>
                        <h3 style='color: #4CAF50; font-size: 24px; margin-bottom: 20px;'>Reset Password</h3>
                    </td>
                </tr>
                <tr>
                    <td style='pading: 10px 0;'>
                        <p style='color: #333; font-size: 16px;'>
                            If you requested to change your password, please click the link below.
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style='padding: 20px 0;'>
                        <a href='{resetPasswordURL}'' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-decoration: none; border-radius: 20px; display: inline-block;'>
                            Reset Password
                        </a>
                    </td>
                </tr>
                <tr>
                    <td style='padding: 10px 0;'>
                        <p style='color: #333; font-size: 14px;'>
                            If you didn't request a password reset, please ignore this email.
                        </p>
                    </td>
                </tr>
                <tr>
                    <td style='padding-top: 20px;'>
                        <p style='color: #666; font-size: 14px;'>
                            Best regards,<br>Library
                        </p>
                    </td>
                </tr>
            </table>
        </div>
        </body>
    </html>
";
                
                var email = new Email()
                {
                    Subject = "Reset Password",
                    Receiver = model.Email,
                    Body = body
                };
                EmailSettings.SendEmail(email);
                return View("CheckEmail");

            }
            TempData["Error"] = "Email not exist";

            return RedirectToAction("ForgotPassword",model);

        }

        public IActionResult CheckEmail()
        {
            return View();
        }

        public IActionResult ChangePassword(string email,string token)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    TempData["ChangePassword"] = "You changed password successfully";

                    return RedirectToAction(nameof(Login));
                }
                
            }
            ViewBag.Error = "Error when trying to change password";
            return View(model);
        }

    }
}
