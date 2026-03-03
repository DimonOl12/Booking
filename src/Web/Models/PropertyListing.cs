namespace Web.Models
{
    /// <summary>
    /// Represents a property listing. Replace mock data source with DB repository when ready.
    /// </summary>
    public class PropertyListing
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public string City { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Type { get; set; } = ""; // Hotel, Apartment, Hostel, etc.

        public decimal RatingScore { get; set; }
        public string RatingLabel { get; set; } = "";
        public int ReviewsCount { get; set; }

        public decimal PricePerNight { get; set; }
        public decimal? OriginalPricePerNight { get; set; } // filled when discounted

        public string Description { get; set; } = "";
        public bool HasFreeWifi { get; set; }
        public bool HasFreeParking { get; set; }
        public bool HasPool { get; set; }
        public bool HasBreakfast { get; set; }
        public int RoomsLeft { get; set; }
    }
}
