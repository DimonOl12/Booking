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

        // ── Messages ─────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult Messages() => View();

        // ── Bookings ─────────────────────────────────────────────────────────────
        [Authorize]
        [HttpGet]
        public IActionResult Bookings() => View();

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

            // Mock data — replace with real DB queries when property store exists.
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
                        Text = "Смачні сніданки, які були різні на перший та другий день. У ванній кімнаті була вся необхідна косметика для гігієни. Нам тільки не вистачало зубних щіток, бо забули з собою взяти). Дуже чисто та все приміщення приємно пахне. Дуже гарний інтер\u2019єр, в ньому приємно знаходитись."
                    },
                    new RealtorReview
                    {
                        Author = "Олексій Куліш",
                        Country = "Україна",
                        Date = "15 січня 2026 року",
                        Text = "Помешкання чисте, затишне та повністю відповідає опису. Є все необхідне для комфортного відпочинку — зручне ліжко, обладнана кухня, швидкий Wi-Fi. Розташування чудове: тихий район, поруч магазини та транспорт."
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
            // Placeholder: remove incomplete registration from store when implemented
            return RedirectToAction("RealtorDashboard");
        }

        // ── Realtor Tab Stubs (to be filled with content as pages arrive) ─────────
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
                Id = 1,
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
                    new PropertyBookingRow { BookingId = "#2457", Property = "Urban Light № 45", GuestName = "Софія Смітт",    Action = "Бронювання", Status = "Cancelled", CheckIn = "25.02.2026 15:00", CheckOut = "26.02.2026 08:00", Amount = "UAH 1 800" },
                    new PropertyBookingRow { BookingId = "#2451", Property = "Urban Light № 45", GuestName = "Павло Селях",    Action = "Бронювання", Status = "Completed", CheckIn = "01.02.2026 15:00", CheckOut = "06.02.2026 08:00", Amount = "UAH 3 600" },
                },
                Reviews = new List<RealtorReview>
                {
                    new RealtorReview { Author = "Юлія Бойко",   Country = "Україна", Date = "25 січня 2026 року",  Text = "Смачні сніданки, які були різні на перший та другий день. У ванній кімнаті була вся необхідна косметика для гігієни. Нам тільки не вистачало зубних щіток, бо забули з собою взяти). Дуже чисто та все приміщення приємно пахне. Дуже гарний інтер'єр, в ньому приємно знаходитись." },
                    new RealtorReview { Author = "Марина Коваль", Country = "Україна", Date = "21 грудня 2025 року", Text = "Чудові апартаменти! Все нове, сучасне та доглянуте. Було комфортно перебувати навіть довший час. Особливо сподобалась тиша та зручне розташування. Рекомендую для пар і ділових поїздок. Заселення безконтактне, інструкції зрозумілі. В апартаментах тепло, чисто й затишно. Є все необхідне для комфортного перебування. Дякуємо за гостинність!" },
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
                    new RatingCategory { Name = "Персонал",                      Score = 9.5m },
                    new RatingCategory { Name = "Зручності",                     Score = 9.3m },
                    new RatingCategory { Name = "Розташування",                  Score = 9.7m },
                    new RatingCategory { Name = "Комфорт",                       Score = 9.5m },
                    new RatingCategory { Name = "Співвідношення ціна/якість",    Score = 9.1m },
                    new RatingCategory { Name = "Чистота",                       Score = 9.4m },
                    new RatingCategory { Name = "Безкоштовний Wi-Fi",            Score = 9.1m },
                },
                MonthlyRatings = new List<decimal> { 9.0m, 9.2m, 8.9m, 9.3m, 9.4m, 9.6m, 9.5m, 9.4m, 9.6m, 9.5m, 9.7m, 9.3m },
                LastReview = new RealtorGuestReview
                {
                    GuestName     = "Юлія Бойко",
                    GuestLocation = "Україна, Київ",
                    PropertyName  = "Urban Light",
                    Stay          = "3 ночі · січень 2026",
                    Guests        = "1 дорослий",
                    ReviewTitle   = "Я залишилася задоволена та рекомендую",
                    ReviewParagraphs = new List<string>
                    {
                        "Смачні сніданки, які були різні на перший та другий день. У ванній кімнаті була вся необхідна косметика для гігієни. Нам тільки не вистачало зубних щіток, бо забули з собою взяти)",
                        "Дуже чисто та все приміщення приємно пахне. Дуже гарний інтер'єр, в ньому приємно знаходитись.",
                        "Навіть під час відключення світла в місті, в готелі все працювало та було дуже тепло. Це був наш перший візит до Львова і нам залишаться приємні враження та спогади. Зручне розташування, ходили пішки на площу ринок, реберню, океанаріум та музеї.",
                        "Так як жили в люксі, то у вартість входила година у спа — діти оцінили джакузі та сауну. Там теж чисто та дуже затишно, можна було скористатися рушниками та халатами.",
                    },
                    Rating      = 9.6m,
                    RatingLabel = "Чудово",
                    OwnerReply  = "Юліє, щиро дякуємо за такий детальний і теплий відгук! Нам дуже приємно, що вам сподобалося проживання, сніданки та атмосфера апартаментів. Раді, що навіть у складних умовах перебування було комфортним і залишило приємні враження від Львова.\n\nОбов'язково врахуємо ваше зауваження щодо зубних щіток, щоб зробити сервіс ще зручнішим. Будемо раді знову вітати вас у наших апартаментах!",
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
        public IActionResult RealtorProfile()
        {
            return View();
        }

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

            if (!_users.ValidatePassword(model.Email, model.Password))
            {
                ViewBag.Error = "Схоже, введені дані з помилкою. Перевірте і спробуйте знову.";
                return View(model);
            }

            var user = _users.FindByEmail(model.Email)!;
            if (!user.IsRealtor)
            {
                ViewBag.Error = "Цей акаунт не є акаунтом партнера. Будь ласка, скористайтеся звичайним входом.";
                return View(model);
            }

            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

        // ── Realtor Register Step 1 ───────────────────────────────────────────────
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

            if (_users.FindByEmail(model.Email) != null)
            {
                ViewBag.Error = "Ця електронна адреса вже зареєстрована.";
                return View(model);
            }

            TempData["RltEmail"] = model.Email;
            return RedirectToAction("RealtorRegisterStep2");
        }

        // ── Realtor Register Step 2 ───────────────────────────────────────────────
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
            if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName) || string.IsNullOrWhiteSpace(model.PhoneNumber))
            {
                ViewBag.Error = "Будь ласка, заповніть всі поля.";
                return View(model);
            }

            TempData["RltEmail"] = model.Email;
            TempData["RltFirstName"] = model.FirstName;
            TempData["RltLastName"] = model.LastName;
            TempData["RltPhone"] = model.PhoneNumber;
            return RedirectToAction("RealtorRegisterStep3");
        }

        // ── Realtor Register Step 3 ───────────────────────────────────────────────
        [HttpGet]
        public IActionResult RealtorRegisterStep3()
        {
            var email = TempData.Peek("RltEmail") as string;
            if (string.IsNullOrEmpty(email)) return RedirectToAction("RealtorRegisterStep1");
            return View(new RealtorRegisterStep3ViewModel
            {
                Email = email,
                FirstName = TempData.Peek("RltFirstName") as string ?? "",
                LastName = TempData.Peek("RltLastName") as string ?? "",
                PhoneNumber = TempData.Peek("RltPhone") as string ?? "",
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RealtorRegisterStep3(RealtorRegisterStep3ViewModel model)
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

            var code = _users.CreatePendingRealtorRegistration(
                model.Email, model.Password, model.FirstName, model.LastName, model.PhoneNumber);

            TempData["RltConfirmCode"] = code;
            TempData["RltConfirmEmail"] = model.Email;
            return RedirectToAction("RealtorRegisterStep4");
        }

        // ── Realtor Register Step 4 ───────────────────────────────────────────────
        [HttpGet]
        public IActionResult RealtorRegisterStep4()
        {
            var email = TempData.Peek("RltConfirmEmail") as string;
            if (string.IsNullOrEmpty(email)) return RedirectToAction("RealtorRegisterStep1");

            var demoCode = TempData["RltConfirmCode"] as string;
            TempData["RltConfirmCode"] = demoCode;
            ViewBag.DemoCode = demoCode;

            return View(new ConfirmEmailViewModel { Email = email });
        }

        // ── Realtor ConfirmEmail ──────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealtorConfirmEmail(ConfirmEmailViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Code))
            {
                ViewBag.Error = "Введіть код підтвердження.";
                return View("RealtorRegisterStep4", model);
            }

            if (!_users.ConfirmRegistration(model.Email, model.Code))
            {
                ViewBag.Error = "Невірний або прострочений код. Спробуйте ще раз.";
                return View("RealtorRegisterStep4", model);
            }

            var user = _users.FindByEmail(model.Email)!;
            await SignInUser(user);
            return RedirectToAction("Index", "Home");
        }

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
                new("IsRealtor", user.IsRealtor ? "true" : "false"),
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
