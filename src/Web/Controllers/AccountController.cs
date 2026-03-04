using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserStore _users;

        public AccountController(IUserStore users)
        {
            _users = users;
        }

        // ── Login ────────────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.Error = "Будь ласка, заповніть всі поля.";
                return View(model);
            }

            if (!_users.ValidatePassword(model.Email, model.Password))
            {
                ViewBag.Error = "Схоже, введені дані з помилкою. Перевірте і спробуйте знову.";
                return View(model);
            }

            var user = _users.FindByEmail(model.Email)!;
            await SignInUser(user);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // ── Register ─────────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.Error = "Будь ласка, заповніть всі поля.";
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Error = "Паролі не співпадають.";
                return View(model);
            }

            if (model.Password.Length < 6)
            {
                ViewBag.Error = "Пароль має містити не менше 6 символів.";
                return View(model);
            }

            if (_users.FindByEmail(model.Email) != null)
            {
                ViewBag.Error = "Ця електронна адреса вже зареєстрована.";
                return View(model);
            }

            // Generate confirmation code and store pending registration
            var code = _users.CreatePendingRegistration(model.Email, model.Password);

            // In production you'd send an email. For demo we pass code via TempData.
            TempData["ConfirmCode"] = code; // shown in view for demo purposes
            TempData["ConfirmEmail"] = model.Email;

            return View("RegisterConfirm", new ConfirmEmailViewModel { Email = model.Email });
        }

        // ── ConfirmEmail ─────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Code))
            {
                ViewBag.Error = "Введіть код підтвердження.";
                return View("RegisterConfirm", model);
            }

            if (!_users.ConfirmRegistration(model.Email, model.Code))
            {
                ViewBag.Error = "Невірний або прострочений код. Спробуйте ще раз.";
                return View("RegisterConfirm", model);
            }

            var user = _users.FindByEmail(model.Email)!;
            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        // ── Logout ───────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // ── Profile ──────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var user = _users.FindByEmail(email);
            if (user == null) return RedirectToAction("Login");

            return View(BuildProfileVm(user));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var user = _users.FindByEmail(email);
            if (user == null) return RedirectToAction("Login");

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DisplayName = model.DisplayName;
            user.PhoneNumber = model.PhoneNumber;
            user.Citizenship = model.Citizenship;
            user.Gender = model.Gender;
            user.Country = model.Country;
            user.City = model.City;
            user.PostalCode = model.PostalCode;
            user.Address = model.Address;
            user.BirthDay = model.BirthDay;
            user.BirthMonth = model.BirthMonth;
            user.BirthYear = model.BirthYear;
            user.PassportFirstName = model.PassportFirstName;
            user.PassportLastName = model.PassportLastName;
            user.PassportNumber = model.PassportNumber;
            user.PassportExpiry = model.PassportExpiry;
            user.PassportNationality = model.PassportNationality;
            _users.Update(user);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await SignInUser(user);

            var vm = BuildProfileVm(user);
            vm.SaveSuccess = "Профіль успішно збережено!";
            return View(vm);
        }

        // ── ForgotPassword Step 1 ─────────────────────────────────────────────────
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Введіть електронну адресу.";
                return View();
            }

            // Always create token (don't reveal if email exists)
            if (_users.FindByEmail(email) != null)
            {
                var token = _users.CreatePasswordResetToken(email);
                TempData["ResetToken"] = token;
            }

            TempData["ResetEmail"] = email;
            return RedirectToAction("ForgotPasswordStep2");
        }

        // ── ForgotPassword Step 2 ─────────────────────────────────────────────────
        [HttpGet]
        public IActionResult ForgotPasswordStep2()
        {
            var email = TempData.Peek("ResetEmail") as string;
            if (string.IsNullOrEmpty(email)) return RedirectToAction("ForgotPassword");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPasswordStep2(string password, string confirmPassword)
        {
            var email = TempData.Peek("ResetEmail") as string;
            var token = TempData.Peek("ResetToken") as string;

            if (string.IsNullOrEmpty(email)) return RedirectToAction("ForgotPassword");

            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                ViewBag.Error = "Будь ласка, заповніть всі поля.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Паролі не співпадають.";
                return View();
            }

            if (password.Length < 6)
            {
                ViewBag.Error = "Пароль має містити не менше 6 символів.";
                return View();
            }

            if (!string.IsNullOrEmpty(token))
                _users.ResetPassword(email, token, password);

            TempData.Remove("ResetEmail");
            TempData.Remove("ResetToken");

            return RedirectToAction("ForgotPasswordStep3");
        }

        // ── ForgotPassword Step 3 ─────────────────────────────────────────────────
        [HttpGet]
        public IActionResult ForgotPasswordStep3() => View();

        // ── Helper ───────────────────────────────────────────────────────────────
        private async Task SignInUser(AppUser user)
        {
            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            if (string.IsNullOrEmpty(fullName)) fullName = user.Email;

            var initials = BuildInitials(user);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, fullName),
                new(ClaimTypes.Email, user.Email),
                new("FirstName", user.FirstName ?? ""),
                new("LastName", user.LastName ?? ""),
                new("Initials", initials),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });
        }

        private static string BuildInitials(AppUser user)
        {
            var f = user.FirstName?.Length > 0 ? user.FirstName[0].ToString().ToUpper() : "";
            var l = user.LastName?.Length > 0 ? user.LastName[0].ToString().ToUpper() : "";
            if (!string.IsNullOrEmpty(f + l)) return f + l;
            return user.Email.Length > 0 ? user.Email[0].ToString().ToUpper() : "?";
        }

        private static UserProfileViewModel BuildProfileVm(AppUser user) => new()
        {
            Email = user.Email,
            FirstName = user.FirstName ?? "",
            LastName = user.LastName ?? "",
            DisplayName = user.DisplayName ?? "",
            PhoneNumber = user.PhoneNumber ?? "",
            Citizenship = user.Citizenship ?? "",
            Gender = user.Gender ?? "",
            Country = user.Country ?? "",
            City = user.City ?? "",
            PostalCode = user.PostalCode ?? "",
            Address = user.Address ?? "",
            BirthDay = user.BirthDay ?? "",
            BirthMonth = user.BirthMonth ?? "",
            BirthYear = user.BirthYear ?? "",
            PassportFirstName = user.PassportFirstName ?? "",
            PassportLastName = user.PassportLastName ?? "",
            PassportNumber = user.PassportNumber ?? "",
            PassportExpiry = user.PassportExpiry ?? "",
            PassportNationality = user.PassportNationality ?? "",
        };
    }
}
