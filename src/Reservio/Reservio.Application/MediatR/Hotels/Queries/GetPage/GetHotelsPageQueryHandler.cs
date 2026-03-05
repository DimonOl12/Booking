using Reservio.Application.Interfaces;
using Reservio.Application.MediatR.Hotels.Queries.Shared;
using Reservio.Application.Models.Pagination;
using MediatR;

namespace Reservio.Application.MediatR.Hotels.Queries.GetPage;

public class GetHotelsPageQueryHandler(
	IPaginationService<HotelVm, GetHotelsPageQuery> pagination
) : IRequestHandler<GetHotelsPageQuery, PageVm<HotelVm>> {

	public Task<PageVm<HotelVm>> Handle(GetHotelsPageQuery request, CancellationToken cancellationToken) =>
		pagination.GetPageAsync(request, cancellationToken);
}
