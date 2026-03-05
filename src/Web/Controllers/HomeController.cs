using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Property(int id, string checkIn = "", string checkOut = "",
            int adults = 2, int children = 0, int rooms = 1)
        {
            var allListings = GetMockListings();
            var listing = allListings.FirstOrDefault(l => l.Id == id);
            if (listing == null) return NotFound();

            var similar = allListings
                .Where(l => l.Id != id && l.City == listing.City)
                .Take(4)
                .ToList();

            var vm = new PropertyDetailViewModel
            {
                Listing        = listing,
                SimilarListings = similar,
                CheckIn  = checkIn,
                CheckOut = checkOut,
                Adults   = adults,
                Children = children,
                Rooms    = rooms,
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Realtor()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string city = "", string checkIn = "", string checkOut = "",
            int adults = 2, int children = 0, int rooms = 1,
            string? filterType = null, decimal minPrice = 400, decimal maxPrice = 6500,
            string sortBy = "recommended")
        {
            // ── Mock data ────────────────────────────────────────────────────────────
            // TODO: Replace with real DB/repository call when database is connected.
            // Example: var listings = await _listingRepository.SearchAsync(city, checkIn, checkOut, adults, children, rooms);
            var allListings = GetMockListings();

            // Simple city filter (case-insensitive, trimmed)
            var filtered = allListings
                .Where(l => string.IsNullOrWhiteSpace(city) ||
                            l.City.Contains(city.Trim(), StringComparison.OrdinalIgnoreCase) ||
                            l.Location.Contains(city.Trim(), StringComparison.OrdinalIgnoreCase))
                .Where(l => l.PricePerNight >= minPrice && l.PricePerNight <= maxPrice)
                .ToList();

            if (!string.IsNullOrEmpty(filterType))
                filtered = filtered.Where(l => l.Type == filterType).ToList();

            filtered = sortBy switch
            {
                "price_asc" => filtered.OrderBy(l => l.PricePerNight).ToList(),
                "price_desc" => filtered.OrderByDescending(l => l.PricePerNight).ToList(),
                "rating" => filtered.OrderByDescending(l => l.RatingScore).ToList(),
                _ => filtered // recommended — keep original order
            };

            // All city-matching listings (no price/type/sort filter) for client-side JS
            var cityListings = allListings
                .Where(l => string.IsNullOrWhiteSpace(city) ||
                            l.City.Contains(city.Trim(), StringComparison.OrdinalIgnoreCase) ||
                            l.Location.Contains(city.Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();

            var vm = new SearchViewModel
            {
                City = city,
                CheckIn = checkIn,
                CheckOut = checkOut,
                Adults = adults,
                Children = children,
                Rooms = rooms,
                FilterType = filterType,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortBy = sortBy,
                TotalFound = filtered.Count,
                Listings = filtered,
                AllListings = cityListings
            };

            return View(vm);
        }

        // ── Mock data source ─────────────────────────────────────────────────────────
        // Swap this method out with a DB call later.
        private static List<PropertyListing> GetMockListings() =>
        [
            new()
            {
                Id = 1,
                Name = "Emily Resort",
                Location = "Львів",
                City = "Львів",
                ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945?w=500",
                Type = "Готель",
                RatingScore = 9.5m,
                RatingLabel = "Чудово",
                ReviewsCount = 2999,
                PricePerNight = 12400,
                Description = "Помешкання Emily Resort розташовано у Львові, за 8,6 км від пам'ятки \"Палац Семенських-Левицьких\". До послуг гостей відкритий плавальний басейн, приватна парковка та тераса.",
                HasFreeWifi = true,
                HasFreeParking = true,
                HasPool = true,
                RoomsLeft = 1
            },
            new()
            {
                Id = 2,
                Name = "Rudolfo Hotel",
                Location = "Центр Львова • Площа Ринок",
                City = "Львів",
                ImageUrl = "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?w=500",
                Type = "Готель",
                RatingScore = 9.5m,
                RatingLabel = "Чудово",
                ReviewsCount = 318,
                PricePerNight = 3240,
                Description = "Прокиньтесь у самому серці чарівного Старого міста Львова – лише за кілька кроків від Оперного театру та площі \"Ринок\", готель Rudolfo пропонує модний комфорт та автентичний міський дух.",
                HasFreeWifi = true,
                HasFreeParking = false,
                HasBreakfast = true,
                RoomsLeft = 1
            },
            new()
            {
                Id = 3,
                Name = "Apart-HOTEL GALLERY 21",
                Location = "Львів • 600 м від центру",
                City = "Львів",
                ImageUrl = "https://images.unsplash.com/photo-1631049307264-da0ec9d70304?w=500",
                Type = "Апартаменти",
                RatingScore = 9.5m,
                RatingLabel = "Чудово",
                ReviewsCount = 139,
                PricePerNight = 1560,
                OriginalPricePerNight = 1950,
                Description = "Помешкання Apart-HOTEL GALLERY 21 розташовано в місті Львів, за 4 хв. пішки від центру. Сучасний дизайн та повний набір зручностей для комфортного проживання.",
                HasFreeWifi = true,
                HasFreeParking = true,
                RoomsLeft = 1
            },
            new()
            {
                Id = 4,
                Name = "Атлас Делюкс",
                Location = "Львів • Тихий район",
                City = "Львів",
                ImageUrl = "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?w=500",
                Type = "Готель",
                RatingScore = 9.6m,
                RatingLabel = "Чудово",
                ReviewsCount = 242,
                PricePerNight = 3990,
                Description = "Готель Atlas Deluxe — елегантний відпочинок у серці Львова. 5 хвилин прогулянки — і ви на площі Ринок, а за 10 хвилин відкривається парк імені Івана Франка.",
                HasFreeWifi = true,
                HasFreeParking = false,
                HasBreakfast = true,
                RoomsLeft = 2
            },
            new()
            {
                Id = 5,
                Name = "Cities Gallery Apart-hotel",
                Location = "Центр Львова, Львів",
                City = "Львів",
                ImageUrl = "https://images.unsplash.com/photo-1560347876-aeef00ee58a1?w=500",
                Type = "Апартаменти",
                RatingScore = 9.6m,
                RatingLabel = "Чудово",
                ReviewsCount = 400,
                PricePerNight = 2275,
                Description = "У помешканні Cities Gallery Apart-hotel, яке розміщено в місті Львів, за 2 хв. від центру — ідеально для тривалого перебування.",
                HasFreeWifi = true,
                HasFreeParking = true,
                RoomsLeft = 3
            },
            new()
            {
                Id = 6,
                Name = "Jam Apartments Miskevycha",
                Location = "Центр Львова, Львів",
                City = "Львів",
                ImageUrl = "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=500",
                Type = "Апартаменти",
                RatingScore = 9.6m,
                RatingLabel = "Чудово",
                ReviewsCount = 109,
                PricePerNight = 2546,
                Description = "Апарт-готель Jam Apartments Miskevycha у Львові пропонує комфортабельні номери з кондиціонером, кухонним куточком та окремою ванною кімнатою. Безкоштовний Wi-Fi у кожному номері.",
                HasFreeWifi = true,
                HasFreeParking = false,
                RoomsLeft = 1
            },
            new()
            {
                Id = 7,
                Name = "7 Apartments",
                Location = "Центр Львова, Львів",
                City = "Львів",
                ImageUrl = "https://images.unsplash.com/photo-1522771739844-6a9f6d5f14af?w=500",
                Type = "Апартаменти",
                RatingScore = 9.3m,
                RatingLabel = "Чудово",
                ReviewsCount = 174,
                PricePerNight = 3182,
                Description = "У помешканні 7 Apartments, яке розміщено в центрі міста Львів, за 7 хв. від площі Ринок — простора та затишна квартира для двох.",
                HasFreeWifi = true,
                HasFreeParking = true,
                RoomsLeft = 2
            },
            new()
            {
                Id = 8,
                Name = "Premier Palace Kyiv",
                Location = "Центр Києва, вул. Шевченка",
                City = "Київ",
                ImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=500",
                Type = "Готель",
                RatingScore = 9.4m,
                RatingLabel = "Чудово",
                ReviewsCount = 1240,
                PricePerNight = 8900,
                Description = "Premier Palace — найвідоміший готель Києва у самому серці міста. Розкішні номери, ресторан та СПА-центр до ваших послуг.",
                HasFreeWifi = true,
                HasFreeParking = true,
                HasPool = true,
                HasBreakfast = true,
                RoomsLeft = 3
            },
            new()
            {
                Id = 9,
                Name = "Ribas Hotel Odesa",
                Location = "Одеса • Дерибасівська",
                City = "Одеса",
                ImageUrl = "https://images.unsplash.com/photo-1445019980597-93fa8acb246c?w=500",
                Type = "Готель",
                RatingScore = 9.1m,
                RatingLabel = "Чудово",
                ReviewsCount = 520,
                PricePerNight = 4800,
                OriginalPricePerNight = 5600,
                Description = "Boutique-готель Ribas у пішій доступності від Дерибасівської та морського берегу. Авторський дизайн, ресторан та тераса з видом на місто.",
                HasFreeWifi = true,
                HasFreeParking = false,
                HasBreakfast = true,
                RoomsLeft = 2
            },
            new()
            {
                Id = 10,
                Name = "Буковель Хаус Шале",
                Location = "Буковель, Карпати",
                City = "Буковель",
                ImageUrl = "https://images.unsplash.com/photo-1518780664697-55e3ad937233?w=500",
                Type = "Вілла",
                RatingScore = 9.8m,
                RatingLabel = "Відмінно",
                ReviewsCount = 87,
                PricePerNight = 6200,
                Description = "Затишне шале в горах Буковеля з камінком, сауною та видом на засніжені вершини. Ідеально для зимового відпочинку або літнього трекінгу.",
                HasFreeWifi = true,
                HasFreeParking = true,
                HasPool = false,
                RoomsLeft = 1
            }
        ];
    }
}
