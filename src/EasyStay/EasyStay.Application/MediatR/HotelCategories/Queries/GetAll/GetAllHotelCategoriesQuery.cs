using EasyStay.Application.MediatR.HotelCategories.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.HotelCategories.Queries.GetAll;

public class GetAllHotelCategoriesQuery : IRequest<IEnumerable<HotelCategoryVm>> { }
