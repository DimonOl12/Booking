using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Web.Models;
using Web.Services;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IReservioApiClient _api;
        private readonly LocalUserStore _localUsers;

        public AccountController(IReservioApiClient api, LocalUserStore localUsers)
        {
            _api = api;
            _localUsers = localUsers;
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

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ViewBag.Error = "Введіть електронну адресу.";
                return View(model);
            }

            if (!IsValidEmail(model.Email))
            {
                ViewBag.Error = "Невірний формат email. Перевірте і спробуйте знову.";
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.Error = "Введіть пароль.";
                return View(model);
            }

            // Local-only auth (no backend API required)
            var local = _localUsers.Authenticate(model.Email.Trim(), model.Password);
            if (local == null)
            {
                ViewBag.Error = "Невірний email або пароль.";
                return View(model);
            }
            await SignInLocalAsync(local.Value.Email, local.Value.FirstName, local.Value.LastName);
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        // ── Google OAuth ──────────────────────────────────────────────────────────
        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback", "Account")
            };
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (!result.Succeeded)
                return RedirectToAction("Login");

            var claims = result.Principal?.Claims.ToList() ?? [];

            var email     = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value     ?? "";
            var firstName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? "";
            var lastName  = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value   ?? "";
            var fullName  = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value      ?? email;

            if (string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(fullName))
            {
                var parts = fullName.Split(' ', 2);
                firstName = parts[0];
                lastName  = parts.Length > 1 ? parts[1] : "";
            }

            var initials = "";
            if (firstName.Length > 0) initials += char.ToUpper(firstName[0]);
            if (lastName.Length  > 0) initials += char.ToUpper(lastName[0]);
            if (string.IsNullOrEmpty(initials) && email.Length > 0)
                initials = char.ToUpper(email[0]).ToString();

            var cookieClaims = new List<Claim>
            {
                new(ClaimTypes.Name,           fullName),
                new(ClaimTypes.Email,          email),
                new(ClaimTypes.NameIdentifier, email),
                new(ClaimTypes.Role,           "Customer"),
                new("FirstName",               firstName),
                new("LastName",                lastName),
                new("Initials",                initials),
                new("IsRealtor",               "false"),
                new("email",                   email),
                new("photo",                   ""),
            };

            var identity  = new ClaimsIdentity(cookieClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30) });

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
            // Email
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ViewBag.Error = "Введіть електронну адресу.";
                return View(model);
            }
            if (!IsValidEmail(model.Email))
            {
                ViewBag.Error = "Невірний формат email. Наприклад: name@example.com";
                return View(model);
            }
            if (model.Email.Length > 100)
            {
                ViewBag.Error = "Email занадто довгий (максимум 100 символів).";
                return View(model);
            }

            // First/Last name
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                ViewBag.Error = "Введіть ім'я.";
                return View(model);
            }
            if (!IsValidName(model.FirstName))
            {
                ViewBag.Error = "Ім'я може містити лише літери, пробіли та дефіси.";
                return View(model);
            }
            if (!string.IsNullOrWhiteSpace(model.LastName) && !IsValidName(model.LastName))
            {
                ViewBag.Error = "Прізвище може містити лише літери, пробіли та дефіси.";
                return View(model);
            }

            // Password
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.Error = "Введіть пароль.";
                return View(model);
            }
            if (model.Password.Length < 8)
            {
                ViewBag.Error = "Пароль має містити мінімум 8 символів.";
                return View(model);
            }
            if (model.Password.Length > 128)
            {
                ViewBag.Error = "Пароль занадто довгий (максимум 128 символів).";
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                ViewBag.Error = "Підтвердіть пароль.";
                return View(model);
            }
            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Error = "Паролі не співпадають.";
                return View(model);
            }

            var firstName = model.FirstName.Trim();
            var lastName  = string.IsNullOrWhiteSpace(model.LastName) ? "" : model.LastName.Trim();

            // Local-only registration (no backend API required)
            if (_localUsers.Exists(model.Email.Trim()))
            {
                ViewBag.Error = "Користувач з таким email вже існує.";
                return View(model);
            }
            _localUsers.Register(model.Email.Trim(), model.Password, firstName, lastName);
            await SignInLocalAsync(model.Email.Trim(), firstName, lastName);
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
            var infoTask  = _api.GetCustomerInfoAsync();
            var gendersTask = _api.GetGendersAsync();
            var citizenTask = _api.GetCitizenshipsAsync();
            var citiesTask  = _api.GetCitiesAsync();
            await Task.WhenAll(infoTask, gendersTask, citizenTask, citiesTask);

            var vm = BuildProfileVmFromApi(infoTask.Result);
            vm.GenderOptions      = BuildSelectList(gendersTask.Result, vm.GenderId);
            vm.CitizenshipOptions = BuildSelectList(citizenTask.Result, vm.CitizenshipId);
            vm.CityOptions        = BuildCitySelectList(citiesTask.Result, vm.CityId);
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            // Reload dropdown options for re-display
            var genders      = await _api.GetGendersAsync();
            var citizenships = await _api.GetCitizenshipsAsync();
            var cities       = await _api.GetCitiesAsync();
            model.GenderOptions      = BuildSelectList(genders, model.GenderId);
            model.CitizenshipOptions = BuildSelectList(citizenships, model.CitizenshipId);
            model.CityOptions        = BuildCitySelectList(cities, model.CityId);
            model.Email              = User.FindFirst("email")?.Value ?? model.Email;

            var errors = new List<string>();

            // 1. Update FirstName / LastName via UpdateUserInfo
            var firstName = model.FirstName?.Trim() ?? "";
            var lastName  = model.LastName?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(firstName))
            {
                model.SaveError = "Ім'я не може бути порожнім.";
                return View(model);
            }

            var (newToken, nameError) = await _api.UpdateUserInfoAsync(
                firstName, string.IsNullOrWhiteSpace(lastName) ? "-" : lastName, model.Email);

            if (newToken != null)
            {
                // Re-sign in with updated token so claims reflect new name
                await SignInFromApiTokenAsync(newToken);
                model.Email = User.FindFirst("email")?.Value ?? model.Email;
            }
            else if (nameError != null)
            {
                errors.Add(nameError);
            }

            // 2. Update customer-specific info (phone, dob, address, city, gender, citizenship)
            int? bd = int.TryParse(model.BirthDay,   out var d) ? d : null;
            int? bm = int.TryParse(model.BirthMonth, out var m) ? m : null;
            int? by = int.TryParse(model.BirthYear,  out var y) ? y : null;

            if (model.CitizenshipId > 0 || model.GenderId > 0 || model.CityId > 0
                || !string.IsNullOrWhiteSpace(model.PhoneNumber)
                || !string.IsNullOrWhiteSpace(model.Address))
            {
                var (ok, infoError) = await _api.UpdateCustomerInfoAsync(
                    model.PhoneNumber ?? "",
                    model.Address ?? "",
                    model.CitizenshipId,
                    model.GenderId,
                    model.CityId,
                    bd, bm, by);

                if (!ok && infoError != null)
                    errors.Add(infoError);
            }

            if (errors.Count > 0)
                model.SaveError = string.Join(" ", errors);
            else
                model.SaveSuccess = "Профіль успішно збережено!";

            return View(model);
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
        public IActionResult ForgotPasswordStep2(string password, string confirmPassword)
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
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                ViewBag.Error = "Введіть електронну адресу.";
                return View(model);
            }
            if (!IsValidEmail(model.Email))
            {
                ViewBag.Error = "Невірний формат email.";
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.Error = "Введіть пароль.";
                return View(model);
            }

            // Local-only auth (no backend API required)
            var local = _localUsers.Authenticate(model.Email.Trim(), model.Password);
            if (local == null)
            {
                ViewBag.Error = "Невірний email або пароль.";
                return View(model);
            }
            await SignInLocalAsync(local.Value.Email, local.Value.FirstName, local.Value.LastName, isRealtor: true);
            return RedirectToAction("RealtorDashboard");
        }

        // ── Add Property ──────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddProperty()
        {
            if (User.FindFirst("IsRealtor")?.Value != "true")
                return RedirectToAction("RealtorDashboard");

            var catsTask  = _api.GetHotelCategoriesAsync();
            var amensTask = _api.GetHotelAmenitiesAsync();
            var citiesTask = _api.GetCitiesAsync();
            await Task.WhenAll(catsTask, amensTask, citiesTask);

            var vm = new AddPropertyViewModel
            {
                CategoryOptions = BuildSelectList(catsTask.Result, 0),
                AmenityOptions  = amensTask.Result
                    .Select(a => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(a.Name, a.Id.ToString()))
                    .ToList(),
                CityOptions = BuildCitySelectList(citiesTask.Result, 0),
            };
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProperty(AddPropertyViewModel model)
        {
            if (User.FindFirst("IsRealtor")?.Value != "true")
                return RedirectToAction("RealtorDashboard");

            async Task<AddPropertyViewModel> Reload()
            {
                var cT = _api.GetHotelCategoriesAsync();
                var aT = _api.GetHotelAmenitiesAsync();
                var ciT = _api.GetCitiesAsync();
                await Task.WhenAll(cT, aT, ciT);
                model.CategoryOptions = BuildSelectList(cT.Result, model.CategoryId);
                model.AmenityOptions  = aT.Result
                    .Select(a => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(a.Name, a.Id.ToString()))
                    .ToList();
                model.CityOptions = BuildCitySelectList(ciT.Result, model.CityId);
                return model;
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                model.SaveError = "Введіть назву помешкання.";
                return View(await Reload());
            }
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                model.SaveError = "Введіть опис помешкання.";
                return View(await Reload());
            }
            if (model.CategoryId <= 0)
            {
                model.SaveError = "Оберіть категорію.";
                return View(await Reload());
            }
            if (model.CityId <= 0)
            {
                model.SaveError = "Оберіть місто.";
                return View(await Reload());
            }
            if (string.IsNullOrWhiteSpace(model.Street))
            {
                model.SaveError = "Введіть назву вулиці.";
                return View(await Reload());
            }
            if (string.IsNullOrWhiteSpace(model.HouseNumber))
            {
                model.SaveError = "Введіть номер будинку.";
                return View(await Reload());
            }
            if (model.Photos == null || model.Photos.Count == 0)
            {
                model.SaveError = "Додайте хоча б одне фото.";
                return View(await Reload());
            }

            var req = new Models.Api.CreateHotelRequest
            {
                Name             = model.Name.Trim(),
                Description      = model.Description?.Trim() ?? "",
                CategoryId       = model.CategoryId,
                ArrivalFrom      = TimeToSeconds(model.ArrivalFrom),
                ArrivalTo        = TimeToSeconds(model.ArrivalTo),
                DepartureFrom    = TimeToSeconds(model.DepartureFrom),
                DepartureTo      = TimeToSeconds(model.DepartureTo),
                Street           = model.Street.Trim(),
                HouseNumber      = model.HouseNumber.Trim(),
                ApartmentNumber  = model.ApartmentNumber?.Trim(),
                CityId           = model.CityId,
                AmenityIds       = model.SelectedAmenityIds ?? [],
                Photos           = model.Photos ?? [],
            };

            var (id, error) = await _api.CreateHotelAsync(req);
            if (id == null)
            {
                model.SaveError = error ?? "Помилка створення помешкання.";
                return View(await Reload());
            }

            TempData["PropertyAdded"] = "true";
            return RedirectToAction("RealtorMyAds");
        }

        private static string TimeToSeconds(string hhmm)
        {
            // accepts "14:00" → "14:00:00" for TimeOnly parsing on backend
            if (string.IsNullOrWhiteSpace(hhmm)) return "00:00:00";
            return hhmm.Contains(':') && hhmm.Split(':').Length == 2
                ? hhmm + ":00"
                : hhmm;
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
            if (!IsValidEmail(model.Email))
            {
                ViewBag.Error = "Невірний формат email. Наприклад: name@example.com";
                return View(model);
            }
            if (model.Email.Length > 100)
            {
                ViewBag.Error = "Email занадто довгий (максимум 100 символів).";
                return View(model);
            }
            TempData["RltEmail"] = model.Email.Trim();
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
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                ViewBag.Error = "Введіть ім'я.";
                return View(model);
            }
            if (!IsValidName(model.FirstName))
            {
                ViewBag.Error = "Ім'я може містити лише літери, пробіли та дефіси.";
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.LastName))
            {
                ViewBag.Error = "Введіть прізвище.";
                return View(model);
            }
            if (!IsValidName(model.LastName))
            {
                ViewBag.Error = "Прізвище може містити лише літери, пробіли та дефіси.";
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                ViewBag.Error = "Введіть номер телефону.";
                return View(model);
            }
            if (!IsValidPhone(model.PhoneNumber))
            {
                ViewBag.Error = "Невірний формат номера телефону. Наприклад: +380501234567";
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
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ViewBag.Error = "Введіть пароль.";
                return View(model);
            }
            if (model.Password.Length < 8)
            {
                ViewBag.Error = "Пароль має містити мінімум 8 символів.";
                return View(model);
            }
            if (model.Password.Length > 128)
            {
                ViewBag.Error = "Пароль занадто довгий (максимум 128 символів).";
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                ViewBag.Error = "Підтвердіть пароль.";
                return View(model);
            }
            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Error = "Паролі не співпадають.";
                return View(model);
            }

            // Local-only registration (no backend API required)
            if (_localUsers.Exists(model.Email))
            {
                ViewBag.Error = "Користувач з таким email вже існує.";
                return View(model);
            }
            _localUsers.Register(model.Email, model.Password, model.FirstName, model.LastName);

            TempData.Remove("RltEmail");
            TempData.Remove("RltFirstName");
            TempData.Remove("RltLastName");
            TempData.Remove("RltPhone");

            await SignInLocalAsync(model.Email, model.FirstName, model.LastName, isRealtor: true);
            return RedirectToAction("RealtorDashboard");
        }

        // ── Realtor Register Step 4 (success page only) ───────────────────────────
        [HttpGet]
        public IActionResult RealtorRegisterStep4()
        {
            return RedirectToAction("RealtorDashboard");
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private async Task SignInLocalAsync(string email, string firstName, string lastName, bool isRealtor = false)
        {
            var fullName = $"{firstName} {lastName}".Trim();
            if (string.IsNullOrEmpty(fullName)) fullName = email;

            var initials = "";
            if (firstName.Length > 0) initials += char.ToUpper(firstName[0]);
            if (lastName.Length  > 0) initials += char.ToUpper(lastName[0]);
            if (string.IsNullOrEmpty(initials) && email.Length > 0)
                initials = char.ToUpper(email[0]).ToString();

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name,           fullName),
                new(ClaimTypes.Email,          email),
                new(ClaimTypes.NameIdentifier, email),
                new(ClaimTypes.Role,           isRealtor ? "Realtor" : "Customer"),
                new("FirstName",               firstName),
                new("LastName",                lastName),
                new("Initials",                initials),
                new("IsRealtor",               isRealtor ? "true" : "false"),
                new("email",                   email),
                new("photo",                   ""),
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30) });
        }

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

        // ── Validation Helpers ────────────────────────────────────────────────────

        private static string GenerateUniqueUserName(string email)
        {
            var local = email.Split('@')[0]
                .Replace(".", "_")
                .Replace("+", "_")
                .Replace("-", "_");
            // Add 4-digit random suffix to avoid clashes with existing or seeded users
            var suffix = Random.Shared.Next(1000, 9999).ToString();
            return $"{local}_{suffix}";
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var trimmed = email.Trim();
            // Basic email regex: must have one @ with non-empty local and domain parts containing a dot
            return Regex.IsMatch(trimmed,
                @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$",
                RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));
        }

        private static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            // Allow unicode letters, spaces, hyphens, apostrophes
            return Regex.IsMatch(name.Trim(),
                @"^[\p{L}\s'\-]+$",
                RegexOptions.None, TimeSpan.FromSeconds(1));
        }

        private static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            // Allow +, digits, spaces, dashes, parentheses; min 7 digits
            var digits = Regex.Replace(phone, @"\D", "");
            return digits.Length >= 7 && digits.Length <= 15 &&
                   Regex.IsMatch(phone.Trim(), @"^[+\d\s\-\(\)]+$");
        }

        private UserProfileViewModel BuildProfileVmFromApi(Models.Api.CustomerInfoApiDto? info)
        {
            var vm = new UserProfileViewModel
            {
                Email     = User.FindFirst("email")?.Value ?? User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                FirstName = User.FindFirst("FirstName")?.Value ?? "",
                LastName  = User.FindFirst("LastName")?.Value ?? "",
            };

            if (info != null)
            {
                vm.Email       = info.Email;
                vm.PhoneNumber = info.PhoneNumber ?? "";
                vm.Address     = info.Address ?? "";

                if (!string.IsNullOrWhiteSpace(info.FullName))
                {
                    var parts    = info.FullName.Split(' ', 2);
                    vm.FirstName = parts[0];
                    vm.LastName  = parts.Length > 1 ? parts[1] : "";
                }

                if (info.DateOfBirth.HasValue && info.DateOfBirth.Value.Year > 1)
                {
                    vm.BirthDay   = info.DateOfBirth.Value.Day.ToString();
                    vm.BirthMonth = info.DateOfBirth.Value.Month.ToString();
                    vm.BirthYear  = info.DateOfBirth.Value.Year.ToString();
                }

                vm.GenderId      = info.Gender?.Id ?? 0;
                vm.CitizenshipId = info.Citizenship?.Id ?? 0;
                vm.CityId        = info.City?.Id ?? 0;
            }

            return vm;
        }

        private static List<SelectListItem> BuildSelectList(
            List<Models.Api.RefItemDto> items, long selectedId)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem("— не вибрано —", "0", selectedId == 0)
            };
            list.AddRange(items.Select(i =>
                new SelectListItem(i.Name, i.Id.ToString(), i.Id == selectedId)));
            return list;
        }

        private static List<SelectListItem> BuildCitySelectList(
            List<Models.Api.RefCityDto> items, long selectedId)
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem("— не вибрано —", "0", selectedId == 0)
            };
            list.AddRange(items.Select(i =>
                new SelectListItem($"{i.Name} ({i.Country.Name})", i.Id.ToString(), i.Id == selectedId)));
            return list;
        }
    }
}
