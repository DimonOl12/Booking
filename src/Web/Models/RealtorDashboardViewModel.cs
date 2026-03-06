namespace Web.Models
{
    public class RealtorDashboardViewModel
    {
        public IncompletePropertyRegistration? IncompleteRegistration { get; set; }
        public RealtorStats? Stats { get; set; }
        public List<RealtorReview> Reviews { get; set; } = new();
    }

    public class IncompletePropertyRegistration
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int ProgressPercent { get; set; }
    }

    public class RealtorStats
    {
        public int ActiveProperties { get; set; }
        public int Bookings { get; set; }
        public int Cancellations { get; set; }
        public int CheckIns { get; set; }
        public int CheckOuts { get; set; }
        public decimal Rating { get; set; }
        public string RatingLabel { get; set; } = string.Empty;
        public int ReviewCount { get; set; }
        public decimal TodayIncome { get; set; }
        public decimal TotalBalance { get; set; }
    }

    public class RealtorReview
    {
        public string Author { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
