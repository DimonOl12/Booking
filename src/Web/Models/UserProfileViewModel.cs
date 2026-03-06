using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models.Api;

namespace Web.Models
{
    public class UserProfileViewModel
    {
        // ── Basic info ────────────────────────────────────────────────────────
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Address { get; set; } = "";
        public string BirthDay { get; set; } = "";
        public string BirthMonth { get; set; } = "";
        public string BirthYear { get; set; } = "";

        // ── IDs for dropdowns (saved to DB) ───────────────────────────────────
        public long GenderId { get; set; }
        public long CitizenshipId { get; set; }
        public long CityId { get; set; }

        // ── Reference lists for <select> options ──────────────────────────────
        public List<SelectListItem> GenderOptions { get; set; } = [];
        public List<SelectListItem> CitizenshipOptions { get; set; } = [];
        public List<SelectListItem> CityOptions { get; set; } = [];

        // ── Passport fields (stored locally, not in API yet) ──────────────────
        public string PassportFirstName { get; set; } = "";
        public string PassportLastName { get; set; } = "";
        public string PassportNumber { get; set; } = "";
        public string PassportExpiry { get; set; } = "";
        public string PassportNationality { get; set; } = "";

        // ── Feedback ──────────────────────────────────────────────────────────
        public string? SaveSuccess { get; set; }
        public string? SaveError { get; set; }
    }
}
