namespace Web.Models
{
    public class RealtorRatingViewModel
    {
        public bool HasRating { get; set; }
        public decimal OverallRating { get; set; }
        public string RatingLabel { get; set; } = string.Empty;
        public int ReviewCount { get; set; }
        public List<RatingCategory> Categories { get; set; } = new();
        public List<decimal> MonthlyRatings { get; set; } = new();
        public RealtorGuestReview? LastReview { get; set; }
    }

    public class RatingCategory
    {
        public string Name { get; set; } = string.Empty;
        public decimal Score { get; set; }
    }

    public class RealtorGuestReview
    {
        public string GuestName { get; set; } = string.Empty;
        public string GuestLocation { get; set; } = string.Empty;
        public string PropertyName { get; set; } = string.Empty;
        public string Stay { get; set; } = string.Empty;
        public string Guests { get; set; } = string.Empty;
        public string ReviewTitle { get; set; } = string.Empty;
        public List<string> ReviewParagraphs { get; set; } = new();
        public decimal Rating { get; set; }
        public string RatingLabel { get; set; } = string.Empty;
        public string OwnerReply { get; set; } = string.Empty;
    }
}
