using Microsoft.AspNetCore.Authentication.Cookies;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Session (stores JWT token from the Reservio API)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Reservio.Session";
    options.IdleTimeout = TimeSpan.FromDays(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.Cookie.Name = "Reservio.Auth";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddHttpContextAccessor();

// Reservio API client (replaces in-memory UserStore)
builder.Services.AddHttpClient<IReservioApiClient, ReservioApiClient>();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
