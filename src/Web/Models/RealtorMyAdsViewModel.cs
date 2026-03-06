namespace Web.Models
{
    public class RealtorMyAdsViewModel
    {
        public List<RealtorPropertyListing> Listings { get; set; } = new();
    }

    public class RealtorPropertyListing
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public string RatingLabel { get; set; } = string.Empty;
        public int ReviewCount { get; set; }
        // Status: Active | Available | Occupied | OnModeration | Rejected
        public string Status { get; set; } = "Active";
        public string? SecondStatus { get; set; }
        public int BookingsCount { get; set; }
        public int MessagesCount { get; set; }
    }
}
