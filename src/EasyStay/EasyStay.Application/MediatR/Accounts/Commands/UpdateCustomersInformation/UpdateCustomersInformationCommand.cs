using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Commands.UpdateCustomersInformation;

public class UpdateCustomersInformationCommand : IRequest {
	public string PhoneNumber { get; set; } = null!;

	public DateOnly DateOfBirth { get; set; }

	public string Address { get; set; } = null!;

	public long CitizenshipId { get; set; }

	public long GenderId { get; set; }

	public long CityId { get; set; }
}
