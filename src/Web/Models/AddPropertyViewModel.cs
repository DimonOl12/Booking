using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models;

public class AddPropertyViewModel
{
    // ── Basic info ────────────────────────────────────────────────────────────
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public long CategoryId { get; set; }

    // ── Check-in / check-out ──────────────────────────────────────────────────
    public string ArrivalFrom { get; set; } = "14:00";
    public string ArrivalTo { get; set; } = "22:00";
    public string DepartureFrom { get; set; } = "08:00";
    public string DepartureTo { get; set; } = "12:00";

    // ── Address ───────────────────────────────────────────────────────────────
    public long CityId { get; set; }
    public string Street { get; set; } = "";
    public string HouseNumber { get; set; } = "";
    public string? ApartmentNumber { get; set; }

    // ── Amenities (IDs submitted as checkboxes) ───────────────────────────────
    public List<long> SelectedAmenityIds { get; set; } = [];

    // ── Photos ────────────────────────────────────────────────────────────────
    public List<IFormFile> Photos { get; set; } = [];

    // ── Select lists (populated from API) ────────────────────────────────────
    public List<SelectListItem> CategoryOptions { get; set; } = [];
    public List<SelectListItem> CityOptions { get; set; } = [];
    public List<SelectListItem> AmenityOptions { get; set; } = [];

    // ── Result ────────────────────────────────────────────────────────────────
    public string? SaveError { get; set; }
}
