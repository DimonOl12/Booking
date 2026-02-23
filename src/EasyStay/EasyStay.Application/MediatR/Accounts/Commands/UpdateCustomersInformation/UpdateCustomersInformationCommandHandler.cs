using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EasyStay.Application.MediatR.Accounts.Commands.UpdateCustomersInformation;

public class UpdateCustomersInformationCommandHandler(
	IEasyStayDbContext context,
	UserManager<User> userManager,
	ICurrentUserService currentUserService
) : IRequestHandler<UpdateCustomersInformationCommand> {

	public async Task Handle(UpdateCustomersInformationCommand request, CancellationToken cancellationToken) {
		var user = await userManager.FindByEmailAsync(currentUserService.GetRequiredUserEmail());

		if (user is not Customer)
			throw new Exception("User is not a customer");

		var customer = (Customer)user;

		customer.DateOfBirth = request.DateOfBirth;
		customer.Address = request.Address;
		customer.CitizenshipId = request.CitizenshipId;
		customer.GenderId = request.GenderId;
		customer.CityId = request.CityId;

		using var transaction = await context.BeginTransactionAsync(cancellationToken);
		try {
			var result = await userManager.UpdateAsync(customer);
			if (!result.Succeeded)
				throw new IdentityException(result, "Failed to update customer information");

			result = await userManager.SetPhoneNumberAsync(customer, request.PhoneNumber);
			if (!result.Succeeded)
				throw new IdentityException(result, "Failed to update customer phone number");

			await transaction.CommitAsync(cancellationToken);
		}
		catch {
			await transaction.RollbackAsync(cancellationToken);
		}
	}
}
