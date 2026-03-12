using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services;

namespace Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IReservioApiClient _api;

    public AdminController(IReservioApiClient api)
    {
        _api = api;
    }

    public async Task<IActionResult> Dashboard()
    {
        var customersTask = _api.GetAdminCustomersAsync(0, 5);
        var realtorsTask  = _api.GetAdminRealtorsAsync(0, 5);
        var hotelsTask    = _api.GetAdminHotelsAsync(pageSize: 1);
        await Task.WhenAll(customersTask, realtorsTask, hotelsTask);

        var vm = new AdminDashboardViewModel
        {
            TotalCustomers  = customersTask.Result.ItemsAvailable,
            TotalRealtors   = realtorsTask.Result.ItemsAvailable,
            TotalHotels     = hotelsTask.Result.ItemsAvailable,
            RecentCustomers = customersTask.Result.Data.ToList(),
            RecentRealtors  = realtorsTask.Result.Data.ToList(),
        };
        return View(vm);
    }

    public async Task<IActionResult> Users(int page = 0, string? search = null, string? status = null)
    {
        const int pageSize = 15;
        bool? isLocked = status switch { "active" => false, "blocked" => true, _ => null };
        var result = await _api.GetAdminCustomersAsync(page, pageSize, search, isLocked);
        var vm = new AdminUsersViewModel
        {
            Users        = result.Data.ToList(),
            TotalCount   = result.ItemsAvailable,
            TotalPages   = result.PagesAvailable,
            CurrentPage  = page,
            Search       = search,
            StatusFilter = status,
            Flash        = TempData["Flash"] as string,
        };
        return View(vm);
    }

    public async Task<IActionResult> Realtors(int page = 0, string? search = null, string? status = null)
    {
        const int pageSize = 15;
        bool? isLocked = status switch { "active" => false, "blocked" => true, _ => null };
        var result = await _api.GetAdminRealtorsAsync(page, pageSize, search, isLocked);
        var vm = new AdminRealtorsViewModel
        {
            Realtors     = result.Data.ToList(),
            TotalCount   = result.ItemsAvailable,
            TotalPages   = result.PagesAvailable,
            CurrentPage  = page,
            Search       = search,
            StatusFilter = status,
            Flash        = TempData["Flash"] as string,
        };
        return View(vm);
    }

    public async Task<IActionResult> Hotels(int page = 0, string? search = null, string? status = null)
    {
        const int pageSize = 15;
        bool? isArchived = status switch { "active" => false, "archived" => true, _ => null };
        var result = await _api.GetAdminHotelsAsync(page, pageSize, search, isArchived);
        var vm = new AdminHotelsViewModel
        {
            Hotels       = result.Data.ToList(),
            TotalCount   = result.ItemsAvailable,
            TotalPages   = result.PagesAvailable,
            CurrentPage  = page,
            Search       = search,
            StatusFilter = status,
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BlockUser(long id, string returnUrl = "/Admin/Users")
    {
        await _api.BlockUserAsync(id);
        TempData["Flash"] = "Користувача заблоковано.";
        return Redirect(returnUrl);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnlockUser(long id, string returnUrl = "/Admin/Users")
    {
        await _api.UnlockUserAsync(id);
        TempData["Flash"] = "Користувача розблоковано.";
        return Redirect(returnUrl);
    }
}
