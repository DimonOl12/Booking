using MediatR;

namespace Reservio.Application.MediatR.Hotels.Queries.GetMaxPrice;

public class GetMaxPriceCommand : IRequest<decimal?> { }

