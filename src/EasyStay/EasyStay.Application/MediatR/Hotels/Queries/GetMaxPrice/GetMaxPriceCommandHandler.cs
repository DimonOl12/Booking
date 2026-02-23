using EasyStay.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Hotels.Queries.GetMaxPrice;

public class GetMaxPriceCommandHandler(
	IEasyStayDbContext context
) : IRequestHandler<GetMaxPriceCommand, decimal?> {

	public Task<decimal?> Handle(GetMaxPriceCommand request, CancellationToken cancellationToken) {
		return context.RoomVariants
			.MaxAsync(
				rv => (decimal?)(rv.DiscountPrice ?? rv.Price),
				cancellationToken
			);
	}
}
