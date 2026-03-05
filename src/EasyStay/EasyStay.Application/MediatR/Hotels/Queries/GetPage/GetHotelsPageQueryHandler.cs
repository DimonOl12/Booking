using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Hotels.Queries.Shared;
using EasyStay.Application.Models.Pagination;
using MediatR;

namespace EasyStay.Application.MediatR.Hotels.Queries.GetPage;

public class GetHotelsPageQueryHandler(
	IPaginationService<HotelVm, GetHotelsPageQuery> pagination
) : IRequestHandler<GetHotelsPageQuery, PageVm<HotelVm>> {

	public Task<PageVm<HotelVm>> Handle(GetHotelsPageQuery request, CancellationToken cancellationToken) =>
		pagination.GetPageAsync(request, cancellationToken);
}