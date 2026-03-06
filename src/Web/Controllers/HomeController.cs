using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Models.Api;
using Web.Services;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReservioApiClient _api;

        public HomeController(IReservioApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Realtor()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            string city = "", string checkIn = "", string checkOut = "",
            int adults = 2, int children = 0, int rooms = 1,
            string? filterType = null, decimal minPrice = 0, decimal maxPrice = 0,
            string sortBy = "recommended")
        {
            List<PropertyListing> allListings;
            try
            {
                var all = await _api.GetHotelsAsync(
                    cityName: string.IsNullOrWhiteSpace(city) ? null : city,
                    pageSize: 100);
                allListings = all.Select(MapToListing).ToList();
            }
            catch
            {
                allListings = [];
            }

            decimal actualMax = allListings.Any() ? allListings.Max(l => l.PricePerNight) : 10000m;
            decimal actualMin = allListings.Any() ? allListings.Min(l => l.PricePerNight) : 0m;

            if (maxPrice == 0) maxPrice = actualMax;
            if (minPrice == 0) minPrice = actualMin;

            var filtered = allListings
                .Where(l => l.PricePerNight >= minPrice && l.PricePerNight <= maxPrice)
                .ToList();

            if (!string.IsNullOrEmpty(filterType))
                filtered = filtered.Where(l => l.Type == filterType).ToList();

            filtered = sortBy switch
            {
                "price_asc"  => filtered.OrderBy(l => l.PricePerNight).ToList(),
                "price_desc" => filtered.OrderByDescending(l => l.PricePerNight).ToList(),
                "rating"     => filtered.OrderByDescending(l => l.RatingScore).ToList(),
                _            => filtered
            };

            var vm = new SearchViewModel
            {
                City        = city,
                CheckIn     = checkIn,
                CheckOut    = checkOut,
                Adults      = adults,
                Children    = children,
                Rooms       = rooms,
                FilterType  = filterType,
                MinPrice    = minPrice,
                MaxPrice    = maxPrice,
                SortBy      = sortBy,
                TotalFound  = filtered.Count,
                Listings    = filtered,
                AllListings = allListings
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Property(
            long id, string checkIn = "", string checkOut = "",
            int adults = 2, int children = 0, int rooms = 1)
        {
            HotelDetailsApiDto? hotel;
            try { hotel = await _api.GetHotelDetailsAsync(id); }
            catch { hotel = null; }

            if (hotel == null) return NotFound();

            var listing = MapDetailsToListing(hotel);

            List<PropertyListing> similar = [];
            try
            {
                var allCity = await _api.GetHotelsAsync(cityName: hotel.Address.City.Name, pageSize: 20);
                similar = allCity.Where(h => h.Id != hotel.Id).Take(4).Select(MapToListing).ToList();
            }
            catch { }

            var vm = new PropertyDetailViewModel
            {
                Listing         = listing,
                SimilarListings = similar,
                CheckIn         = checkIn,
                CheckOut        = checkOut,
                Adults          = adults,
                Children        = children,
                Rooms           = rooms
            };

            return View(vm);
        }

        // ─── Mapping ──────────────────────────────────────────────────────────

        private PropertyListing MapToListing(HotelApiDto h)
        {
            var photo    = h.Photos.FirstOrDefault()?.Name;
            var imageUrl = photo is not null
                ? _api.BuildImageUrl(photo)
                : "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=500";

            var location = h.Address.City.Name;
            if (!string.IsNullOrEmpty(h.Address.Street))
                location = $"{h.Address.Street} {h.Address.HouseNumber}, {location}";

            var amenities = h.HotelAmenities.Select(a => a.Name.ToLowerInvariant()).ToArray();
            var price     = h.MinPrice ?? 0m;
            var original  = h.MaxPrice.HasValue && h.MaxPrice > h.MinPrice ? (decimal?)h.MaxPrice.Value : null;

            return new PropertyListing
            {
                Id                    = (int)h.Id,
                Name                  = h.Name,
                Location              = location,
                City                  = h.Address.City.Name,
                ImageUrl              = imageUrl,
                Type                  = h.Category.Name,
                RatingScore           = (decimal)h.Rating,
                RatingLabel           = RatingLabel(h.Rating),
                ReviewsCount          = 0,
                PricePerNight         = price,
                OriginalPricePerNight = original,
                Description           = h.Description,
                HasFreeWifi           = amenities.Any(n => n.Contains("wi-fi") || n.Contains("wifi") || n.Contains("інтернет")),
                HasFreeParking        = amenities.Any(n => n.Contains("паркінг") || n.Contains("парковк") || n.Contains("parking")),
                HasPool               = amenities.Any(n => n.Contains("басейн") || n.Contains("pool")),
                HasBreakfast          = amenities.Any(n => n.Contains("сніданок") || n.Contains("breakfast")),
                RoomsLeft             = 1
            };
        }

        private PropertyListing MapDetailsToListing(HotelDetailsApiDto h)
        {
            var photo    = h.Photos.FirstOrDefault()?.Name;
            var imageUrl = photo is not null
                ? _api.BuildImageUrl(photo)
                : "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=500";

            var location = h.Address.City.Name;
            if (!string.IsNullOrEmpty(h.Address.Street))
                location = $"{h.Address.Street} {h.Address.HouseNumber}, {location}";

            var amenities = h.HotelAmenities.Select(a => a.Name.ToLowerInvariant()).ToArray();

            return new PropertyListing
            {
                Id            = (int)h.Id,
                Name          = h.Name,
                Location      = location,
                City          = h.Address.City.Name,
                ImageUrl      = imageUrl,
                Type          = h.Category.Name,
                RatingScore   = (decimal)h.Rating,
                RatingLabel   = RatingLabel(h.Rating),
                ReviewsCount  = 0,
                PricePerNight = h.MinPrice ?? 0m,
                Description   = h.Description,
                HasFreeWifi   = amenities.Any(n => n.Contains("wi-fi") || n.Contains("wifi") || n.Contains("інтернет")),
                HasFreeParking= amenities.Any(n => n.Contains("паркінг") || n.Contains("парковк") || n.Contains("parking")),
                HasPool       = amenities.Any(n => n.Contains("басейн") || n.Contains("pool")),
                HasBreakfast  = amenities.Any(n => n.Contains("сніданок") || n.Contains("breakfast")),
                RoomsLeft     = h.Rooms.Sum(r => r.Quantity)
            };
        }

        private static string RatingLabel(double score) => score switch
        {
            >= 9.5 => "Відмінно",
            >= 9.0 => "Чудово",
            >= 8.0 => "Дуже добре",
            >= 7.0 => "Добре",
            >= 6.0 => "Задовільно",
            _      => "Нижче середнього"
        };
    }
}
