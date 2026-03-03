namespace Web.Models
{
    public class SearchViewModel
    {
        // Search parameters (passed in from query string)
        public string City { get; set; } = "";
        public string CheckIn { get; set; } = "";
        public string CheckOut { get; set; } = "";
        public int Adults { get; set; } = 2;
        public int Children { get; set; } = 0;
        public int Rooms { get; set; } = 1;

        // Filter parameters
        public string? FilterType { get; set; }
        public decimal MinPrice { get; set; } = 400;
        public decimal MaxPrice { get; set; } = 6500;
        public string SortBy { get; set; } = "recommended";

        // Results
        public int TotalFound { get; set; }
        public List<PropertyListing> Listings { get; set; } = new();
        /// <summary>All city-matching listings (unfiltered) — used by client-side JS for real-time filtering.</summary>
        public List<PropertyListing> AllListings { get; set; } = new();

        // Helpers
        public string GuestsLabel =>
            $"{Adults} {PluralAdults(Adults)} · {Children} {PluralChildren(Children)} · {Rooms} номер";

        public string DatesLabel =>
            string.IsNullOrEmpty(CheckIn) ? "Дата заїзду – Дата виїзду"
            : $"{CheckIn} – {CheckOut}";

        private static string PluralAdults(int n) => n == 1 ? "дорослий" : "дорослих";
        private static string PluralChildren(int n) => n == 0 ? "дітей" : n == 1 ? "дитина" : "дітей";
    }
}
