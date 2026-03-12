namespace Web.Models;

public class AdminDashboardViewModel
{
    public int TotalCustomers { get; set; }
    public int TotalRealtors { get; set; }
    public int TotalHotels { get; set; }
    public List<Web.Models.Api.AdminUserDto> RecentCustomers { get; set; } = new();
    public List<Web.Models.Api.AdminUserDto> RecentRealtors { get; set; } = new();
}

public class AdminUsersViewModel
{
    public List<Web.Models.Api.AdminUserDto> Users { get; set; } = new();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public string? Search { get; set; }
    public string? StatusFilter { get; set; }
    public string? Flash { get; set; }
}

public class AdminRealtorsViewModel
{
    public List<Web.Models.Api.AdminUserDto> Realtors { get; set; } = new();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public string? Search { get; set; }
    public string? StatusFilter { get; set; }
    public string? Flash { get; set; }
}

public class AdminHotelsViewModel
{
    public List<Web.Models.Api.HotelApiDto> Hotels { get; set; } = new();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public string? Search { get; set; }
    public string? StatusFilter { get; set; }
    public string? Flash { get; set; }
}
