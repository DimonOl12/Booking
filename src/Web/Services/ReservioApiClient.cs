using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Web.Models.Api;

namespace Web.Services;

public interface IReservioApiClient
{
    // ─── Auth ─────────────────────────────────────────────────────────────────
    Task<(string? Token, string? Error)> SignInAsync(string email, string password);
    Task<(string? Token, string? Error)> RegisterAsync(
        string firstName, string lastName, string email,
        string userName, string password, string type);
    Task<(bool Success, string? Error)> SendResetPasswordEmailAsync(string email);
    Task<(bool Success, string? Error)> ResetPasswordAsync(
        string email, string resetToken, string newPassword);

    // ─── Hotels ───────────────────────────────────────────────────────────────
    Task<List<HotelApiDto>> GetHotelsAsync(
        string? cityName = null, decimal? minPrice = null,
        decimal? maxPrice = null, int pageSize = 50);
    Task<HotelDetailsApiDto?> GetHotelDetailsAsync(long id);
    Task<decimal> GetMaxPriceAsync();

    // ─── Bookings (authenticated) ─────────────────────────────────────────────
    Task<List<BookingApiDto>> GetBookingsAsync();

    // ─── Account (authenticated) ──────────────────────────────────────────────
    Task<CustomerInfoApiDto?> GetCustomerInfoAsync();
    Task<(string? NewToken, string? Error)> UpdateUserInfoAsync(string firstName, string lastName, string email);
    Task<(bool Success, string? Error)> UpdateCustomerInfoAsync(
        string phoneNumber, string address, long citizenshipId, long genderId, long cityId,
        int? birthDay, int? birthMonth, int? birthYear);

    // ─── Reference data (public) ──────────────────────────────────────────────
    Task<List<RefItemDto>> GetGendersAsync();
    Task<List<RefItemDto>> GetCitizenshipsAsync();
    Task<List<RefCityDto>> GetCitiesAsync();
    Task<List<RefItemDto>> GetHotelCategoriesAsync();
    Task<List<RefItemDto>> GetHotelAmenitiesAsync();

    // ─── Hotels (authenticated realtor) ───────────────────────────────────────
    Task<(long? Id, string? Error)> CreateHotelAsync(CreateHotelRequest req);

    // ─── Session helpers ──────────────────────────────────────────────────────
    void SetJwtToken(string token);
    void ClearJwtToken();
    string? GetJwtToken();
    string BuildImageUrl(string? photoName);
}

public class ReservioApiClient : IReservioApiClient
{
    private readonly HttpClient _http;
    private readonly IHttpContextAccessor _accessor;
    private readonly string _baseUrl;
    private const string SessionKey = "ApiJwt";

    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ReservioApiClient(HttpClient http, IHttpContextAccessor accessor, IConfiguration config)
    {
        _http = http;
        _accessor = accessor;
        _baseUrl = (config["ReservioApi:BaseUrl"] ?? "http://localhost:5292").TrimEnd('/');
    }

    // ─── Session ──────────────────────────────────────────────────────────────

    public string? GetJwtToken() => _accessor.HttpContext?.Session.GetString(SessionKey);
    public void SetJwtToken(string token) => _accessor.HttpContext?.Session.SetString(SessionKey, token);
    public void ClearJwtToken() => _accessor.HttpContext?.Session.Remove(SessionKey);

    public string BuildImageUrl(string? photoName) =>
        string.IsNullOrEmpty(photoName) ? "" : $"/images/{photoName}";

    private void AddAuth(HttpRequestMessage req)
    {
        var token = GetJwtToken();
        if (!string.IsNullOrEmpty(token))
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // ─── Auth ─────────────────────────────────────────────────────────────────

    public async Task<(string? Token, string? Error)> SignInAsync(string email, string password)
    {
        try
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["email"] = email,
                ["password"] = password
            });

            using var resp = await _http.PostAsync($"{_baseUrl}/api/Accounts/SignIn", form);
            var body = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                return (null, ParseError(body) ?? "Невірний email або пароль.");

            var result = JsonSerializer.Deserialize<JwtResponse>(body, _json);
            return (result?.Token, null);
        }
        catch (HttpRequestException)
        {
            return (null, "Сервер тимчасово недоступний. Спробуйте пізніше.");
        }
    }

    public async Task<(string? Token, string? Error)> RegisterAsync(
        string firstName, string lastName, string email,
        string userName, string password, string type)
    {
        try
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["firstName"] = firstName,
                ["lastName"] = lastName,
                ["email"] = email,
                ["userName"] = userName,
                ["password"] = password,
                ["type"] = type
            });

            using var resp = await _http.PostAsync($"{_baseUrl}/api/Accounts/Registration", form);
            var body = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
                return (null, ParseError(body) ?? "Помилка реєстрації.");

            var result = JsonSerializer.Deserialize<JwtResponse>(body, _json);
            return (result?.Token, null);
        }
        catch (HttpRequestException)
        {
            return (null, "Сервер тимчасово недоступний. Спробуйте пізніше.");
        }
    }

    public async Task<(bool Success, string? Error)> SendResetPasswordEmailAsync(string email)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(new { email }),
            Encoding.UTF8, "application/json");

        using var resp = await _http.PostAsync($"{_baseUrl}/api/Accounts/SendResetPasswordEmail", content);
        if (resp.IsSuccessStatusCode) return (true, null);
        var body = await resp.Content.ReadAsStringAsync();
        return (false, ParseError(body) ?? "Помилка відправки листа.");
    }

    public async Task<(bool Success, string? Error)> ResetPasswordAsync(
        string email, string resetToken, string newPassword)
    {
        var content = new StringContent(
            JsonSerializer.Serialize(new { email, token = resetToken, newPassword }),
            Encoding.UTF8, "application/json");

        using var resp = await _http.PostAsync($"{_baseUrl}/api/Accounts/ResetPassword", content);
        if (resp.IsSuccessStatusCode) return (true, null);
        var body = await resp.Content.ReadAsStringAsync();
        return (false, ParseError(body) ?? "Помилка зміни пароля.");
    }

    // ─── Hotels ───────────────────────────────────────────────────────────────

    public async Task<List<HotelApiDto>> GetHotelsAsync(
        string? cityName = null, decimal? minPrice = null,
        decimal? maxPrice = null, int pageSize = 50)
    {
        var qs = new List<string>
        {
            $"PageSize={pageSize}",
            "PageIndex=0",
            "IsArchived=false"
        };

        if (!string.IsNullOrWhiteSpace(cityName))
            qs.Add($"Address.City.Name={Uri.EscapeDataString(cityName)}");
        if (minPrice.HasValue)
            qs.Add($"MinPrice={minPrice.Value}");
        if (maxPrice.HasValue)
            qs.Add($"MaxPrice={maxPrice.Value}");

        var url = $"{_baseUrl}/api/Hotels/GetPage?{string.Join("&", qs)}";
        var page = await GetJsonAsync<ApiPage<HotelApiDto>>(url);
        return page?.Data?.ToList() ?? [];
    }

    public async Task<HotelDetailsApiDto?> GetHotelDetailsAsync(long id) =>
        await GetJsonAsync<HotelDetailsApiDto>($"{_baseUrl}/api/Hotels/GetById/{id}");

    public async Task<decimal> GetMaxPriceAsync()
    {
        var result = await GetJsonAsync<decimal>($"{_baseUrl}/api/Hotels/GetMaxPrice");
        return result > 0 ? result : 10000;
    }

    // ─── Bookings ─────────────────────────────────────────────────────────────

    public async Task<List<BookingApiDto>> GetBookingsAsync()
    {
        var url = $"{_baseUrl}/api/Bookings/GetPage?PageSize=50&PageIndex=0";
        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        AddAuth(req);
        using var resp = await _http.SendAsync(req);
        if (!resp.IsSuccessStatusCode) return [];
        var body = await resp.Content.ReadAsStringAsync();
        var page = JsonSerializer.Deserialize<ApiPage<BookingApiDto>>(body, _json);
        return page?.Data?.ToList() ?? [];
    }

    // ─── Account ──────────────────────────────────────────────────────────────

    public async Task<CustomerInfoApiDto?> GetCustomerInfoAsync()
    {
        var url = $"{_baseUrl}/api/Accounts/GetCustomersInformation";
        using var req = new HttpRequestMessage(HttpMethod.Get, url);
        AddAuth(req);
        using var resp = await _http.SendAsync(req);
        if (!resp.IsSuccessStatusCode) return null;
        var body = await resp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CustomerInfoApiDto>(body, _json);
    }

    public async Task<(string? NewToken, string? Error)> UpdateUserInfoAsync(
        string firstName, string lastName, string email)
    {
        try
        {
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(firstName), "firstName");
            form.Add(new StringContent(lastName),  "lastName");
            form.Add(new StringContent(email),      "email");

            using var req = new HttpRequestMessage(HttpMethod.Patch,
                $"{_baseUrl}/api/Accounts/UpdateInfo") { Content = form };
            AddAuth(req);
            using var resp = await _http.SendAsync(req);
            var body = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
                return (null, ParseError(body) ?? "Помилка оновлення імені.");
            var result = JsonSerializer.Deserialize<JwtResponse>(body, _json);
            return (result?.Token, null);
        }
        catch (HttpRequestException)
        {
            return (null, "Сервер тимчасово недоступний.");
        }
    }

    public async Task<(bool Success, string? Error)> UpdateCustomerInfoAsync(
        string phoneNumber, string address, long citizenshipId, long genderId, long cityId,
        int? birthDay, int? birthMonth, int? birthYear)
    {
        try
        {
            DateOnly? dob = null;
            if (birthDay.HasValue && birthMonth.HasValue && birthYear.HasValue)
            {
                try { dob = new DateOnly(birthYear.Value, birthMonth.Value, birthDay.Value); }
                catch { /* invalid date — skip */ }
            }

            var payload = new
            {
                phoneNumber,
                address,
                citizenshipId,
                genderId,
                cityId,
                dateOfBirth = dob.HasValue
                    ? $"{dob.Value.Year:0000}-{dob.Value.Month:00}-{dob.Value.Day:00}"
                    : "0001-01-01"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var req = new HttpRequestMessage(HttpMethod.Patch,
                $"{_baseUrl}/api/Accounts/UpdateCustomersInformation") { Content = content };
            AddAuth(req);
            using var resp = await _http.SendAsync(req);
            if (resp.IsSuccessStatusCode) return (true, null);
            var body = await resp.Content.ReadAsStringAsync();
            return (false, ParseError(body) ?? "Помилка збереження даних.");
        }
        catch (HttpRequestException)
        {
            return (false, "Сервер тимчасово недоступний.");
        }
    }

    // ─── Reference data ───────────────────────────────────────────────────────

    public async Task<List<RefItemDto>> GetGendersAsync() =>
        await GetJsonAsync<List<RefItemDto>>($"{_baseUrl}/api/Genders/GetAll") ?? [];

    public async Task<List<RefItemDto>> GetCitizenshipsAsync() =>
        await GetJsonAsync<List<RefItemDto>>($"{_baseUrl}/api/Citizenships/GetAll") ?? [];

    public async Task<List<RefCityDto>> GetCitiesAsync() =>
        await GetJsonAsync<List<RefCityDto>>($"{_baseUrl}/api/Cities/GetAll") ?? [];

    public async Task<List<RefItemDto>> GetHotelCategoriesAsync() =>
        await GetJsonAsync<List<RefItemDto>>($"{_baseUrl}/api/HotelCategories/GetAll") ?? [];

    public async Task<List<RefItemDto>> GetHotelAmenitiesAsync() =>
        await GetJsonAsync<List<RefItemDto>>($"{_baseUrl}/api/HotelAmenities/GetAll") ?? [];

    public async Task<(long? Id, string? Error)> CreateHotelAsync(CreateHotelRequest r)
    {
        try
        {
            var form = new MultipartFormDataContent();
            form.Add(new StringContent(r.Name),           "Name");
            form.Add(new StringContent(r.Description),    "Description");
            form.Add(new StringContent(r.CategoryId.ToString()), "CategoryId");
            form.Add(new StringContent(r.ArrivalFrom),    "ArrivalTimeUtcFrom");
            form.Add(new StringContent(r.ArrivalTo),      "ArrivalTimeUtcTo");
            form.Add(new StringContent(r.DepartureFrom),  "DepartureTimeUtcFrom");
            form.Add(new StringContent(r.DepartureTo),    "DepartureTimeUtcTo");
            form.Add(new StringContent("false"),           "IsArchived");
            form.Add(new StringContent(r.Street),         "Address.Street");
            form.Add(new StringContent(r.HouseNumber),    "Address.HouseNumber");
            form.Add(new StringContent(r.CityId.ToString()), "Address.CityId");
            if (!string.IsNullOrEmpty(r.ApartmentNumber))
                form.Add(new StringContent(r.ApartmentNumber), "Address.ApartmentNumber");

            foreach (var amenityId in r.AmenityIds)
                form.Add(new StringContent(amenityId.ToString()), "HotelAmenityIds");

            foreach (var photo in r.Photos)
            {
                var photoContent = new StreamContent(photo.OpenReadStream());
                photoContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(photo.ContentType);
                form.Add(photoContent, "Photos", photo.FileName);
            }

            using var req = new HttpRequestMessage(HttpMethod.Post,
                $"{_baseUrl}/api/Hotels/Create") { Content = form };
            AddAuth(req);
            using var resp = await _http.SendAsync(req);
            var body = await resp.Content.ReadAsStringAsync();
            if (!resp.IsSuccessStatusCode)
                return (null, ParseError(body) ?? "Помилка створення помешкання.");
            var id = JsonSerializer.Deserialize<long>(body, _json);
            return (id, null);
        }
        catch (HttpRequestException)
        {
            return (null, "Сервер тимчасово недоступний.");
        }
    }

    // ─── Helpers ──────────────────────────────────────────────────────────────

    private async Task<T?> GetJsonAsync<T>(string url)
    {
        try
        {
            using var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return default;
            var body = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(body, _json);
        }
        catch
        {
            return default;
        }
    }

    private static string? ParseError(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);

            // FluentValidation array: [{"PropertyName":"Email","ErrorMessage":"..."}]
            if (doc.RootElement.ValueKind == JsonValueKind.Array)
            {
                var messages = new List<string>();
                foreach (var item in doc.RootElement.EnumerateArray())
                {
                    if (item.TryGetProperty("ErrorMessage", out var msg))
                    {
                        var text = msg.GetString();
                        if (!string.IsNullOrEmpty(text))
                            messages.Add(TranslateApiError(text));
                    }
                }
                return messages.Count > 0 ? string.Join(" ", messages) : null;
            }

            // Standard error object
            foreach (var key in new[] { "detail", "title", "message", "Message" })
                if (doc.RootElement.TryGetProperty(key, out var prop))
                    return TranslateApiError(prop.GetString() ?? "");
        }
        catch { }
        return null;
    }

    private static string TranslateApiError(string msg) => msg switch
    {
        "There is already a user with this email"    => "Акаунт з такою поштою вже існує.",
        "There is already a user with this username" => "Помилка реєстрації. Спробуйте ще раз.",
        "Email is empty or null"                     => "Введіть електронну адресу.",
        "Email is invalid"                           => "Невірний формат email.",
        "Email is too long"                          => "Email занадто довгий.",
        "Password is empty or null"                  => "Введіть пароль.",
        "Password is too short"                      => "Пароль має містити мінімум 8 символів.",
        "FirstName is empty or null"                 => "Введіть ім'я.",
        "LastName is empty or null"                  => "Введіть прізвище.",
        "Name is empty or null"                      => "Введіть назву помешкання.",
        "Name is too long"                           => "Назва помешкання занадто довга.",
        "Description is empty or null"               => "Введіть опис помешкання.",
        "Description is too long (4000)"             => "Опис помешкання занадто довгий (макс. 4000 символів).",
        "HotelCategory with this id is not exists"   => "Оберіть коректну категорію.",
        "One ore more of photos are invalid"         => "Одне або декілька фото недійсні. Перевірте формат та розмір.",
        _ => msg
    };
}
