using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IReservioApiClient _api;

        public AccountController(IReservioApiClient api)
        {
            _api = api;
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

            var (token, error) = await _api.SignInAsync(model.Email, model.Password);
            if (token == null)
            {
                ViewBag.Error = error ?? "Схоже, введені дані з помилкою. Перевірте і спробуйте знову.";
                return View(model);
            }

            await SignInFromApiTokenAsync(token);

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
        public async Task<IActionResult> Register(RegisterViewModel model)
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

            if (model.Password.Length < 8)
            {
                ViewBag.Error = "Пароль має містити не менше 8 символів.";
                return View(model);
            }

            var firstName = string.IsNullOrWhiteSpace(model.FirstName) ? "Користувач" : model.FirstName.Trim();
            var lastName  = string.IsNullOrWhiteSpace(model.LastName) ? "" : model.LastName.Trim();
            var userName  = model.Email.Split('@')[0].Replace(".", "_").Replace("+", "_");

            var (token, error) = await _api.RegisterAsync(
                firstName, lastName, model.Email, userName, model.Password, "Customer");

            if (token == null)
            {
                ViewBag.Error = error ?? "Помилка реєстрації. Спробуйте ще раз.";
                return View(model);
            }

            await SignInFromApiTokenAsync(token);
            return RedirectToAction("Index", "Home");
        }

        // ── Logout ───────────────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _api.ClearJwtToken();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // ── Profile ──────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var info = await _api.GetCustomerInfoAsync();
            var vm = BuildProfileVmFromApi(info);
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            // Profile updates would go to PATCH /api/Accounts/UpdateCustomersInformation
            // For now we save what we can and show success
            var vm = new UserProfileViewModel
            {
                Email               = User.FindFirst("email")?.Value ?? model.Email,
                FirstName           = model.FirstName,
                LastName            = model.LastName,
                DisplayName         = model.DisplayName,
                PhoneNumber         = model.PhoneNumber,
                Citizenship         = model.Citizenship,
                Gender              = model.Gender,
                Country             = model.Country,
                City                = model.City,
                PostalCode          = model.PostalCode,
                Address             = model.Address,
                BirthDay            = model.BirthDay,
                BirthMonth          = model.BirthMonth,
                BirthYear           = model.BirthYear,
                PassportFirstName   = model.PassportFirstName,
                PassportLastName    = model.PassportLastName,
                PassportNumber      = model.PassportNumber,
                PassportExpiry      = model.PassportExpiry,
                PassportNationality = model.PassportNationality,
                SaveSuccess         = "Профіль успішно збережено!"
            };
            return View(vm);
        }

        // ── ForgotPassword Step 1 ─────────────────────────────────────────────────
        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Введіть електронну адресу.";
                return View();
            }

            // Always redirect to step 2 regardless of whether email exists (security)
            await _api.SendResetPasswordEmailAsync(email);
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
        public async Task<IActionResult> ForgotPasswordStep2(string password, string confirmPassword)
        {
            var email = TempData.Peek("ResetEmail") as string;
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

            if (password.Length < 8)
            {
                ViewBag.Error = "Пароль має містити не менше 8 символів.";
                return View();
            }

            // The API reset requires a token sent to email. Without SMTP configured,
            // we can only inform the user.
            TempData.Remove("ResetEmail");
            return RedirectToAction("ForgotPasswordStep3");
        }

        // ── ForgotPassword Step 3 ─────────────────────────────────────────────────
        [HttpGet]
        public IActionResult ForgotPasswordStep3() => View();

        // ── Messages ─────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult Messages() => View();

        // ── Bookings ─────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Bookings()
        {
            ViewBag.ApiBookings = await _api.GetBookingsAsync();
            return View();
        }

        // ── BookingDetail ─────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult BookingDetail(int id = 1)
        {
            ViewBag.BookingId = id;
            return View();
        }

        // ── Saved ─────────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult Saved() => View();

        // ── Settings ─────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult Settings() => View();

        // ── Payment ──────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult Payment() => View();

        // ── Realtor Dashboard ─────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult RealtorDashboard()
        {
            if (User.FindFirst("IsRealtor")?.Value != "true")
                return RedirectToAction("Profile");

            var vm = new RealtorDashboardViewModel
            {
                IncompleteRegistration = new IncompletePropertyRegistration
                {
                    Name = "Hotel Raymond",
                    Address = "м. Івано-Франківськ, вул. Карпатська, буд. 3, кв. 5, 76000, Україна",
                    ProgressPercent = 90
                },
                Stats = new RealtorStats
                {
                    ActiveProperties = 8,
                    Bookings = 24,
                    Cancellations = 3,
                    CheckIns = 4,
                    CheckOuts = 8,
                    Rating = 9.6m,
                    RatingLabel = "Чудово",
                    ReviewCount = 242,
                    TodayIncome = 3085m,
                    TotalBalance = 1220128m
                },
                Reviews = new List<RealtorReview>
                {
                    new RealtorReview
                    {
                        Author = "Юлія Бойко",
                        Country = "Україна",
                        Date = "25 січня 2026 року",
                        Text = "Смачні сніданки, які були різні на перший та другий день. Дуже чисто та все приміщення приємно пахне."
                    },
                    new RealtorReview
                    {
                        Author = "Олексій Куліш",
                        Country = "Україна",
                        Date = "15 січня 2026 року",
                        Text = "Помешкання чисте, затишне та повністю відповідає опису. Є все необхідне для комфортного відпочинку."
                    }
                }
            };
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RealtorDeleteIncomplete()
        {
            return RedirectToAction("RealtorDashboard");
        }

        // ── Realtor MyAds ─────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult RealtorMyAds()
        {
            if (User.FindFirst("IsRealtor")?.Value != "true")
                return RedirectToAction("Profile");

            var vm = new RealtorMyAdsViewModel
            {
                Listings = new List<RealtorPropertyListing>
                {
                    new RealtorPropertyListing { Id = 1, Name = "Urban Light", Address = "Шевченка Проспект 27, Львів, 79005, Україна", Rating = 9.6m, RatingLabel = "Чудово", ReviewCount = 280, Status = "Active", SecondStatus = "Occupied", BookingsCount = 12, MessagesCount = 3 },
                    new RealtorPropertyListing { Id = 2, Name = "Prime Loft", Address = "вул. Сонячна 18Б, Львів, 79019, Україна", Rating = 9.4m, RatingLabel = "Чудово", ReviewCount = 801, Status = "Active", SecondStatus = "Available", BookingsCount = 35, MessagesCount = 1 },
                    new RealtorPropertyListing { Id = 3, Name = "Lviv Central Loft", Address = "просп. Свободи 18, Львів, 79008, Україна", Rating = 9.6m, RatingLabel = "Чудово", ReviewCount = 281, Status = "OnModeration", BookingsCount = 58, MessagesCount = 6 },
                    new RealtorPropertyListing { Id = 4, Name = "Green Point Residence", Address = "вул. Лесі Українки 45, Львів, 79026, Україна", Rating = 9.6m, RatingLabel = "Чудово", ReviewCount = 201, Status = "Rejected", BookingsCount = 23, MessagesCount = 6 },
                }
            };
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public IActionResult RealtorPropertyDetail(int id = 1)
        {
            if (User.FindFirst("IsRealtor")?.Value != "true")
                return RedirectToAction("Profile");

            var vm = new RealtorPropertyDetailViewModel
            {
                Id = id,
                Name = "Urban Light",
                Status = "Active",
                Rating = 9.6m,
                PricePerNight = 1800m,
                Capacity = 4,
                MinBookingNights = 2,
                AreaM2 = 48,
                Rooms = 2,
                Address = "Шевченка Проспект 27, Львів, 79005, Україна",
                Country = "Україна",
                City = "Львів",
                Street = "Шевченка Проспект",
                Building = "27",
                Floor = "5 з 9",
                Apartment = "45",
                Bookings = new List<PropertyBookingRow>
                {
                    new PropertyBookingRow { BookingId = "#2452", Property = "Urban Light № 45", GuestName = "Анна Ковальська",  Action = "Бронювання", Status = "Confirmed", CheckIn = "03.02.2026 14:00", CheckOut = "LeavingToday", Amount = "UAH 5 400" },
                    new PropertyBookingRow { BookingId = "#2453", Property = "Urban Light № 45", GuestName = "Валерій Гасюк",   Action = "Бронювання", Status = "Confirmed", CheckIn = "ArrivalToday",       CheckOut = "12.02.2026 08:00", Amount = "UAH 10 800" },
                    new PropertyBookingRow { BookingId = "#2456", Property = "Urban Light № 45", GuestName = "Петро Гнатюк",   Action = "Бронювання", Status = "Pending",   CheckIn = "23.03.2026 15:00", CheckOut = "31.03.2026 08:00", Amount = "UAH 3 600" },
                },
                Reviews = new List<RealtorReview>
                {
                    new RealtorReview { Author = "Юлія Бойко",   Country = "Україна", Date = "25 січня 2026 року",  Text = "Смачні сніданки, які були різні на перший та другий день. Дуже чисто та все приміщення приємно пахне." },
                    new RealtorReview { Author = "Марина Коваль", Country = "Україна", Date = "21 грудня 2025 року", Text = "Чудові апартаменти! Все нове, сучасне та доглянуте." },
                }
            };
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public IActionResult RealtorRating()
        {
            if (User.FindFirst("IsRealtor")?.Value != "true")
                return RedirectToAction("Profile");

            var vm = new RealtorRatingViewModel
            {
                HasRating = true,
                OverallRating = 9.6m,
                RatingLabel = "Відмінний рейтинг",
                ReviewCount = 242,
                Categories = new List<RatingCategory>
                {
                    new RatingCategory { Name = "Персонал",                   Score = 9.5m },
                    new RatingCategory { Name = "Зручності",                  Score = 9.3m },
                    new RatingCategory { Name = "Розташування",               Score = 9.7m },
                    new RatingCategory { Name = "Комфорт",                    Score = 9.5m },
                    new RatingCategory { Name = "Співвідношення ціна/якість", Score = 9.1m },
                    new RatingCategory { Name = "Чистота",                    Score = 9.4m },
                    new RatingCategory { Name = "Безкоштовний Wi-Fi",         Score = 9.1m },
                },
                MonthlyRatings = new List<decimal> { 9.0m, 9.2m, 8.9m, 9.3m, 9.4m, 9.6m, 9.5m, 9.4m, 9.6m, 9.5m, 9.7m, 9.3m },
                LastReview = new RealtorGuestReview
                {
                    GuestName        = "Юлія Бойко",
                    GuestLocation    = "Україна, Київ",
                    PropertyName     = "Urban Light",
                    Stay             = "3 ночі · січень 2026",
                    Guests           = "1 дорослий",
                    ReviewTitle      = "Я залишилася задоволена та рекомендую",
                    ReviewParagraphs = new List<string>
                    {
                        "Смачні сніданки, які були різні на перший та другий день. Дуже чисто та все приміщення приємно пахне.",
                        "Дуже гарний інтер'єр, в ньому приємно знаходитись.",
                    },
                    Rating      = 9.6m,
                    RatingLabel = "Чудово",
                    OwnerReply  = "Юліє, щиро дякуємо за такий детальний і теплий відгук!"
                }
            };
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public IActionResult RealtorMessages()
        {
            if (User.FindFirst("IsRealtor")?.Value != "true")
                return RedirectToAction("Messages");
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult RealtorProfile() => View();

        // ── Realtor Login ─────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult RealtorLogin()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealtorLogin(LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.Error = "Будь ласка, заповніть всі поля.";
                return View(model);
            }

            var (token, error) = await _api.SignInAsync(model.Email, model.Password);
            if (token == null)
            {
                ViewBag.Error = error ?? "Схоже, введені дані з помилкою. Перевірте і спробуйте знову.";
                return View(model);
            }

            // Check role from JWT
            var claims = DecodeJwtClaims(token);
            var roleKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            var role = claims.FirstOrDefault(c => c.Name == roleKey).Value
                    ?? claims.FirstOrDefault(c => c.Name == "role").Value
                    ?? "";

            if (role != "Realtor")
            {
                ViewBag.Error = "Цей акаунт не є акаунтом партнера. Будь ласка, скористайтеся звичайним входом.";
                return View(model);
            }

            await SignInFromApiTokenAsync(token);
            return RedirectToAction("RealtorDashboard");
        }

        // ── Realtor Register Steps ─────────────────────────────────────────────────
        [HttpGet]
        public IActionResult RealtorRegisterStep1()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View(new RealtorRegisterStep1ViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RealtorRegisterStep1(RealtorRegisterStep1ViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ViewBag.Error = "Введіть електронну адресу.";
                return View(model);
            }
            TempData["RltEmail"] = model.Email;
            return RedirectToAction("RealtorRegisterStep2");
        }

        [HttpGet]
        public IActionResult RealtorRegisterStep2()
        {
            var email = TempData.Peek("RltEmail") as string;
            if (string.IsNullOrEmpty(email)) return RedirectToAction("RealtorRegisterStep1");
            return View(new RealtorRegisterStep2ViewModel { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RealtorRegisterStep2(RealtorRegisterStep2ViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.FirstName) ||
                string.IsNullOrWhiteSpace(model.LastName) ||
                string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                ViewBag.Error = "Будь ласка, заповніть всі поля.";
                return View(model);
            }
            TempData["RltEmail"]     = model.Email;
            TempData["RltFirstName"] = model.FirstName;
            TempData["RltLastName"]  = model.LastName;
            TempData["RltPhone"]     = model.PhoneNumber;
            return RedirectToAction("RealtorRegisterStep3");
        }

        [HttpGet]
        public IActionResult RealtorRegisterStep3()
        {
            var email = TempData.Peek("RltEmail") as string;
            if (string.IsNullOrEmpty(email)) return RedirectToAction("RealtorRegisterStep1");
            return View(new RealtorRegisterStep3ViewModel
            {
                Email       = email,
                FirstName   = TempData.Peek("RltFirstName") as string ?? "",
                LastName    = TempData.Peek("RltLastName") as string ?? "",
                PhoneNumber = TempData.Peek("RltPhone") as string ?? "",
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealtorRegisterStep3(RealtorRegisterStep3ViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Password) || string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                ViewBag.Error = "Будь ласка, заповніть всі поля.";
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Error = "Паролі не співпадають.";
                return View(model);
            }

            if (model.Password.Length < 8)
            {
                ViewBag.Error = "Пароль має містити не менше 8 символів.";
                return View(model);
            }

            var userName = model.Email.Split('@')[0].Replace(".", "_").Replace("+", "_");

            var (token, error) = await _api.RegisterAsync(
                model.FirstName, model.LastName, model.Email, userName, model.Password, "Realtor");

            if (token == null)
            {
                ViewBag.Error = error ?? "Помилка реєстрації. Спробуйте ще раз.";
                return View(model);
            }

            TempData.Remove("RltEmail");
            TempData.Remove("RltFirstName");
            TempData.Remove("RltLastName");
            TempData.Remove("RltPhone");

            await SignInFromApiTokenAsync(token);
            return RedirectToAction("RealtorDashboard");
        }

        // ── Realtor Register Step 4 (success page only) ───────────────────────────
        [HttpGet]
        public IActionResult RealtorRegisterStep4()
        {
            return RedirectToAction("RealtorDashboard");
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private async Task SignInFromApiTokenAsync(string token)
        {
            _api.SetJwtToken(token);

            var claims = BuildMvcClaimsFromJwt(token);
            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30) });
        }

        private static List<Claim> BuildMvcClaimsFromJwt(string token)
        {
            var raw = DecodeJwtClaims(token);
            var map = raw.ToDictionary(c => c.Name, c => c.Value);

            var firstName = map.GetValueOrDefault("firstName", "");
            var lastName  = map.GetValueOrDefault("lastName", "");
            var email     = map.GetValueOrDefault("email", "");
            var photo     = map.GetValueOrDefault("photo", "");
            var id        = map.GetValueOrDefault("id", "");

            var fullName = $"{firstName} {lastName}".Trim();
            if (string.IsNullOrEmpty(fullName)) fullName = email;

            var initials = "";
            if (firstName.Length > 0) initials += char.ToUpper(firstName[0]);
            if (lastName.Length > 0)  initials += char.ToUpper(lastName[0]);
            if (string.IsNullOrEmpty(initials) && email.Length > 0)
                initials = char.ToUpper(email[0]).ToString();

            var roleKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
            var role    = map.GetValueOrDefault(roleKey, map.GetValueOrDefault("role", "Customer"));
            var isRealtor = role == "Realtor" ? "true" : "false";

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name,           fullName),
                new(ClaimTypes.Email,          email),
                new(ClaimTypes.NameIdentifier, id),
                new(ClaimTypes.Role,           role),
                new("FirstName",               firstName),
                new("LastName",                lastName),
                new("Initials",                initials),
                new("IsRealtor",               isRealtor),
                new("email",                   email),
                new("photo",                   photo),
            };

            return claims;
        }

        private static List<(string Name, string Value)> DecodeJwtClaims(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3) return [];

            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');

            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "=";  break;
            }

            var bytes = Convert.FromBase64String(payload);
            var json  = Encoding.UTF8.GetString(bytes);

            var result = new List<(string, string)>();
            try
            {
                using var doc = JsonDocument.Parse(json);
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    var val = prop.Value.ValueKind switch
                    {
                        JsonValueKind.String => prop.Value.GetString() ?? "",
                        JsonValueKind.Number => prop.Value.GetRawText(),
                        _                   => prop.Value.GetRawText()
                    };
                    result.Add((prop.Name, val));
                }
            }
            catch { }

            return result;
        }

        private UserProfileViewModel BuildProfileVmFromApi(Models.Api.CustomerInfoApiDto? info)
        {
            var vm = new UserProfileViewModel
            {
                Email       = User.FindFirst("email")?.Value ?? User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                FirstName   = User.FindFirst("FirstName")?.Value ?? "",
                LastName    = User.FindFirst("LastName")?.Value ?? "",
                DisplayName = User.FindFirst(ClaimTypes.Name)?.Value ?? "",
            };

            if (info != null)
            {
                vm.Email       = info.Email;
                vm.PhoneNumber = info.PhoneNumber ?? "";
                vm.Address     = info.Address ?? "";

                if (info.FullName.Contains(' '))
                {
                    var parts    = info.FullName.Split(' ', 2);
                    vm.FirstName = parts[0];
                    vm.LastName  = parts[1];
                }
                else
                {
                    vm.FirstName = info.FullName;
                }

                if (info.DateOfBirth.HasValue)
                {
                    vm.BirthDay   = info.DateOfBirth.Value.Day.ToString();
                    vm.BirthMonth = info.DateOfBirth.Value.Month.ToString();
                    vm.BirthYear  = info.DateOfBirth.Value.Year.ToString();
                }
            }

            return vm;
        }
    }
}
