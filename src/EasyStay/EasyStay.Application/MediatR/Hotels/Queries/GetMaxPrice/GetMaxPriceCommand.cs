using MediatR;

namespace EasyStay.Application.MediatR.Hotels.Queries.GetMaxPrice;

public class GetMaxPriceCommand : IRequest<decimal?> { }
