namespace Web.Models
{
    public class RealtorPropertyDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "Active";
        public decimal Rating { get; set; }
        public decimal PricePerNight { get; set; }
        public int Capacity { get; set; }
        public int MinBookingNights { get; set; }
        public int AreaM2 { get; set; }
        public int Rooms { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Building { get; set; } = string.Empty;
        public string Floor { get; set; } = string.Empty;
        public string Apartment { get; set; } = string.Empty;
        public List<PropertyBookingRow> Bookings { get; set; } = new();
        public List<RealtorReview> Reviews { get; set; } = new();
    }

    public class PropertyBookingRow
    {
        public string BookingId { get; set; } = string.Empty;
        public string Property { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public string Action { get; set; } = "Бронювання";
        // Confirmed | Pending | Cancelled | Completed
        public string Status { get; set; } = string.Empty;
        public string CheckIn { get; set; } = string.Empty;
        // ArrivalToday | LeavingToday | or date string
        public string CheckOut { get; set; } = string.Empty;
        public string Amount { get; set; } = string.Empty;
    }
}
