using ETICARET.WEBUI.EmailServices;
using ETICARET.WEBUI.Identity;
using ETICARET.WEBUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ETICARET.WEBUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View(new RegisterModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser()
            {
                UserName = model.Username,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //Generate Token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code

                });
                //Send Email
                string siteUrl = "https://localhost:7144";
                string activeUrl = $"{siteUrl}{callbackUrl}";

                string body = $"Merhaba {model.Username};<br><br>Hesabınızı onaylamak için <a href='{activeUrl}' target='_blank'> tıklayınız</a>";
                MailHelper.SendEmail(body, model.Email, "Hesap Aktifleştirme");

                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu.Lütfen tekrar deneyiniz.");
            return View(model);
        }

        public IActionResult Login(string ReturnUrl=null)
        {
            return View(new LoginModel()
            {
                ReturnUrl=ReturnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            ModelState.Remove("ReturnUrl");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            var user2 = await _userManager.FindByNameAsync(model.Email);

            if (user == null && user2==null)
            {
                ModelState.AddModelError("", "Bu kullanıcı ile daha önce kayıt oluşturulmamıştır.");
                return View(model);
            }

            if(!await _userManager.IsEmailConfirmedAsync(user == null ? user2 : user))
            {
                ModelState.AddModelError("", "Lütfen hesabınızı email il onaylayınız.");
                return View(model);
            }

            var result = await _signManager.PasswordSignInAsync(user==null?user2:user, model.Password, true, true);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");
            }
            ModelState.AddModelError("LoginError", "Email veya şifreniz hatalıdır.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if(userId==null || token == null)
            {
                TempData["message"] = "Geçersiz Token";
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    TempData["message"] = "Hesabınız Onaylandı";
                    return View();
                }
            }

            TempData["message"] = "Hesabınız Onaylanmadı.";
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(Email);

            if (user == null) { return View(); }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                token = code

            });
            //Send Email
            string siteUrl = "https://localhost:7144";
            string activeUrl = $"{siteUrl}{callbackUrl}";

            string body = $"Parolanızı yenilemek için <a href='{activeUrl}' target='_blank'> tıklayınız</a>";
            MailHelper.SendEmail(body, Email, "ShopApp Şifre Resetleme");
            return RedirectToAction("Login","Account");
        }

        public IActionResult ResetPassword(string token)
        {
            if (token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordModel() { Token = token };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) { return RedirectToAction("Index", "Home"); }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
