namespace Web.Models;

public class PropertyDetailViewModel
{
    public PropertyListing Listing { get; set; } = new();
    public List<PropertyListing> SimilarListings { get; set; } = new();

    // Carried search params (for the search bar on the detail page)
    public string CheckIn   { get; set; } = "";
    public string CheckOut  { get; set; } = "";
    public int    Adults    { get; set; } = 2;
    public int    Children  { get; set; } = 0;
    public int    Rooms     { get; set; } = 1;
}
