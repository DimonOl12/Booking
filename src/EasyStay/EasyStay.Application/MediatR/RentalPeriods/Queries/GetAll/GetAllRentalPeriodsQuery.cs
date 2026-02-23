using EasyStay.Application.MediatR.RentalPeriods.Queries.Shared;
using MediatR;

namespace EasyStay.Application.MediatR.RentalPeriods.Queries.GetAll;

public class GetAllRentalPeriodsQuery : IRequest<IEnumerable<RentalPeriodVm>> { }
