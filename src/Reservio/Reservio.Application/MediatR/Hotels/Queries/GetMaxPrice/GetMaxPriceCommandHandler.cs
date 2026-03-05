using Reservio.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Hotels.Queries.GetMaxPrice;

public class GetMaxPriceCommandHandler(
	IReservioDbContext context
) : IRequestHandler<GetMaxPriceCommand, decimal?> {

	public Task<decimal?> Handle(GetMaxPriceCommand request, CancellationToken cancellationToken) {
		return context.RoomVariants
			.MaxAsync(
				rv => (decimal?)(rv.DiscountPrice ?? rv.Price),
				cancellationToken
			);
	}
}

