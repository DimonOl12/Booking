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

            if (allListings.Count == 0)
                allListings = GetDemoListings();

            decimal actualMax = allListings.Any() ? allListings.Max(l => l.PricePerNight) : 10000m;
            decimal actualMin = allListings.Any() ? allListings.Min(l => l.PricePerNight) : 0m;

            if (maxPrice == 0) maxPrice = actualMax;
            if (minPrice == 0) minPrice = actualMin;

            var filtered = allListings
                .Where(l => l.PricePerNight >= minPrice && l.PricePerNight <= maxPrice)
                .ToList();

            if (!string.IsNullOrEmpty(city))
                filtered = filtered
                    .Where(l => l.City.Contains(city, StringComparison.OrdinalIgnoreCase)
                             || l.Location.Contains(city, StringComparison.OrdinalIgnoreCase))
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

            PropertyListing listing;
            List<PropertyListing> similar;

            if (hotel == null)
            {
                var demo = GetDemoListings();
                var found = demo.FirstOrDefault(d => d.Id == (int)id);
                if (found == null) return NotFound();
                listing = found;
                similar = demo.Where(d => d.Id != (int)id).Take(4).ToList();
            }
            else
            {
                listing = MapDetailsToListing(hotel);
                similar = [];
                try
                {
                    var allCity = await _api.GetHotelsAsync(cityName: hotel.Address.City.Name, pageSize: 20);
                    similar = allCity.Where(h => h.Id != hotel.Id).Take(4).Select(MapToListing).ToList();
                }
                catch { }
            }

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

        private static List<PropertyListing> GetDemoListings() =>
        [
            new() { Id=1, Name="Панорамні апартаменти Kyiv Sky", Location="вул. Хрещатик 22, Київ", City="Київ",
                ImageUrl="https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=500&q=80",
                Type="Апартаменти", RatingScore=9.8m, RatingLabel="Відмінно", ReviewsCount=134,
                PricePerNight=6500, HasFreeWifi=true, HasBreakfast=false, HasFreeParking=false, RoomsLeft=3,
                Description="Сучасні апартаменти з панорамним видом на місто у самому центрі Києва." },

            new() { Id=2, Name="Готель Прем'єр Палац", Location="вул. Шевченка 5, Київ", City="Київ",
                ImageUrl="https://images.unsplash.com/photo-1551882547-ff40c63fe5fa?w=500&q=80",
                Type="Готель", RatingScore=9.2m, RatingLabel="Чудово", ReviewsCount=310,
                PricePerNight=8900, HasFreeWifi=true, HasBreakfast=true, HasFreeParking=true, RoomsLeft=5,
                Description="Розкішний готель у серці столиці зі зручним доступом до ключових пам'яток." },

            new() { Id=3, Name="Апартаменти біля ратуші", Location="пл. Ринок 1, Львів", City="Львів",
                ImageUrl="https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=500&q=80",
                Type="Апартаменти", RatingScore=9.4m, RatingLabel="Відмінно", ReviewsCount=78,
                PricePerNight=3200, HasFreeWifi=true, HasBreakfast=false, HasFreeParking=false, RoomsLeft=2,
                Description="Затишні апартаменти з видом на площу Ринок у старому місті Львова." },

            new() { Id=4, Name="Котедж з каміном у Карпатах", Location="вул. Гірська 14, Яремче", City="Яремче",
                ImageUrl="https://images.unsplash.com/photo-1510798831971-661eb04b3739?w=500&q=80",
                Type="Котедж", RatingScore=9.6m, RatingLabel="Відмінно", ReviewsCount=55,
                PricePerNight=8100, HasFreeWifi=true, HasBreakfast=false, HasFreeParking=true, RoomsLeft=1,
                Description="Дерев'яний котедж з каміном серед карпатських лісів, ідеально для відпочинку взимку." },

            new() { Id=5, Name="Вілла Брава біля моря", Location="вул. Морська 7, Одеса", City="Одеса",
                ImageUrl="https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=500&q=80",
                Type="Вілла", RatingScore=9.1m, RatingLabel="Чудово", ReviewsCount=89,
                PricePerNight=11200, HasFreeWifi=true, HasBreakfast=false, HasPool=true, HasFreeParking=true, RoomsLeft=1,
                Description="Просторий будинок з басейном та прямим виходом до моря." },

            new() { Id=6, Name="Бутік-готель Дніпро Плаза", Location="пр. Яворницького 30, Дніпро", City="Дніпро",
                ImageUrl="https://images.unsplash.com/photo-1566073771259-6a8506099945?w=500&q=80",
                Type="Готель", RatingScore=8.8m, RatingLabel="Дуже добре", ReviewsCount=143,
                PricePerNight=4750, HasFreeWifi=true, HasBreakfast=true, HasFreeParking=true, RoomsLeft=8,
                Description="Стильний бутік-готель у центрі Дніпра з чудовим сервісом та сучасним дизайном." },

            new() { Id=7, Name="Hostel Sky Bukovel", Location="вул. Поляничанська 12, Буковель", City="Буковель",
                ImageUrl="https://images.unsplash.com/photo-1537225228614-56cc3556d7ed?w=500&q=80",
                Type="Хостел", RatingScore=8.5m, RatingLabel="Дуже добре", ReviewsCount=201,
                PricePerNight=1800, HasFreeWifi=true, HasBreakfast=true, HasFreeParking=false, RoomsLeft=10,
                Description="Молодіжний хостел поруч із гірськолижними підйомниками Буковеля." },

            new() { Id=8, Name="Глемпінг Карпатська Зірка", Location="с. Поляниця, Івано-Франківська обл.", City="Буковель",
                ImageUrl="https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=500&q=80",
                Type="Глемпінг", RatingScore=9.5m, RatingLabel="Відмінно", ReviewsCount=47,
                PricePerNight=5600, HasFreeWifi=false, HasBreakfast=true, HasFreeParking=true, RoomsLeft=4,
                Description="Розкішні намети-будиночки серед Карпат для незабутнього відпочинку на природі." },

            new() { Id=9, Name="Resort & SPA Золота Підкова", Location="вул. Стрийська 45, Львів", City="Львів",
                ImageUrl="https://images.unsplash.com/photo-1540541338537-c5d1cc0a1c6e?w=500&q=80",
                Type="Курортний готель", RatingScore=9.3m, RatingLabel="Чудово", ReviewsCount=265,
                PricePerNight=12500, HasFreeWifi=true, HasBreakfast=true, HasPool=true, HasFreeParking=true, RoomsLeft=6,
                Description="Преміум курортний готель із СПА-комплексом, басейном і відновлювальними процедурами." },

            new() { Id=10, Name="Мансарда на Подолі", Location="вул. Сагайдачного 18, Київ", City="Київ",
                ImageUrl="https://images.unsplash.com/photo-1493809842364-78817add7ffb?w=500&q=80",
                Type="Апартаменти", RatingScore=9.0m, RatingLabel="Чудово", ReviewsCount=62,
                PricePerNight=3900, HasFreeWifi=true, HasBreakfast=false, HasFreeParking=false, RoomsLeft=1,
                Description="Стильна мансарда з терасою та видом на Дніпро в культурному районі Поділ." },
        ];
    }
}
