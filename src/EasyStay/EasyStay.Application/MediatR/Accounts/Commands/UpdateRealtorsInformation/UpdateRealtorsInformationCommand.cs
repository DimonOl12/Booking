using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Commands.UpdateRealtorsInformation;

public class UpdateRealtorsInformationCommand : IRequest {
	public string Description { get; set; } = null!;

	public string PhoneNumber { get; set; } = null!;

	public DateOnly DateOfBirth { get; set; }

	public string Address { get; set; } = null!;

	public long CitizenshipId { get; set; }

	public long GenderId { get; set; }

	public long CityId { get; set; }
}
