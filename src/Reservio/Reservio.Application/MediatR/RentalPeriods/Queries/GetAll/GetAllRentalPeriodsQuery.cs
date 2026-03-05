using Reservio.Application.MediatR.RentalPeriods.Queries.Shared;
using MediatR;

namespace Reservio.Application.MediatR.RentalPeriods.Queries.GetAll;

public class GetAllRentalPeriodsQuery : IRequest<IEnumerable<RentalPeriodVm>> { }

