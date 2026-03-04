namespace Web.Models
{
    public class UserProfileViewModel
    {
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Citizenship { get; set; } = "";
        public string Gender { get; set; } = "";
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string Address { get; set; } = "";
        public string BirthDay { get; set; } = "";
        public string BirthMonth { get; set; } = "";
        public string BirthYear { get; set; } = "";
        public string PassportFirstName { get; set; } = "";
        public string PassportLastName { get; set; } = "";
        public string PassportNumber { get; set; } = "";
        public string PassportExpiry { get; set; } = "";
        public string PassportNationality { get; set; } = "";
        public string? SaveSuccess { get; set; }
    }
}
