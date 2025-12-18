using Company.G05.DAL.Models;
using Company.G05.PL.DTOs;
using Company.G05.PL.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.G05.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SignUp

        // P@ssW0rd

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if(user is null)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if(user is null)
                    {
                        // Register
                        user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree,
                        };
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid SignUp !");
            }

            return View(model);
        }

        #endregion

        #region SignIn

        // P@ssW0rd

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInDTO model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if(User is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (flag)
                    {
                        // Sign In 
                        var result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid Login !");
            }

            return View(model);
        }
        #endregion

        #region SignOut

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Forget Password

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendResetPasswordURL(ForgetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if(User is not null)
                {
                    // Generate Token

                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);

                    // Create URL

                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    // Create Email
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };

                    // Send Email
                    var flag = EmailSittings.SendEmail(email);
                    if (flag)
                    {
                        return RedirectToAction("CheckYourInbox"); 
                    }
                }
            }
            ModelState.AddModelError("", "Invalid Reset Password Operation !");
            return View("ForgetPassword", model);
        }

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion

        #region Reset Password

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;

                if (email is null || token is null) return BadRequest("Invalid Operations");
                var user = await _userManager.FindByEmailAsync(email);

                if(user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("SignIn");
                    }
                }

                ModelState.AddModelError("", "Invalid Reset Password Operation");
            }
            return View();
        }

        #endregion

    }
}
