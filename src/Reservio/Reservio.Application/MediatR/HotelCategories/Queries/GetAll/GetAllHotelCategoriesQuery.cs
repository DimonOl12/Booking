using Reservio.Application.MediatR.HotelCategories.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.HotelCategories.Queries.GetAll;

public class GetAllHotelCategoriesQuery : IRequest<IEnumerable<HotelCategoryVm>> { }

