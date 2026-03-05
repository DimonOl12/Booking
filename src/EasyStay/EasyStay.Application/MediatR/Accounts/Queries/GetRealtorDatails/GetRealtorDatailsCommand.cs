using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Queries.GetRealtorDatails;

public class GetRealtorDatailsCommand : IRequest<RealtorDatailsVm> {
	public long Id { get; set; }
}
