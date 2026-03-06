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
        string.IsNullOrEmpty(photoName) ? "" : $"{_baseUrl}/images/{photoName}";

    private void AddAuth(HttpRequestMessage req)
    {
        var token = GetJwtToken();
        if (!string.IsNullOrEmpty(token))
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // ─── Auth ─────────────────────────────────────────────────────────────────

    public async Task<(string? Token, string? Error)> SignInAsync(string email, string password)
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

    public async Task<(string? Token, string? Error)> RegisterAsync(
        string firstName, string lastName, string email,
        string userName, string password, string type)
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
            foreach (var key in new[] { "detail", "title", "message", "Message" })
                if (doc.RootElement.TryGetProperty(key, out var prop))
                    return prop.GetString();
        }
        catch { }
        return null;
    }
}
