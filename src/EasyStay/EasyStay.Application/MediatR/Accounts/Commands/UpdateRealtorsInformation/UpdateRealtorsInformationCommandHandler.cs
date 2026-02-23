using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EasyStay.Application.MediatR.Accounts.Commands.UpdateRealtorsInformation;

public class UpdateRealtorsInformationCommandHandler(
	IEasyStayDbContext context,
	UserManager<User> userManager,
	ICurrentUserService currentUserService
) : IRequestHandler<UpdateRealtorsInformationCommand> {

	public async Task Handle(UpdateRealtorsInformationCommand request, CancellationToken cancellationToken) {
		var user = await userManager.FindByEmailAsync(currentUserService.GetRequiredUserEmail());

		if (user is not Realtor)
			throw new Exception("User is not a realtor");

		var realtor = (Realtor)user;

		realtor.Description = request.Description;
		realtor.DateOfBirth = request.DateOfBirth;
		realtor.Address = request.Address;
		realtor.CitizenshipId = request.CitizenshipId;
		realtor.GenderId = request.GenderId;
		realtor.CityId = request.CityId;

		using var transaction = await context.BeginTransactionAsync(cancellationToken);
		try {
			var result = await userManager.UpdateAsync(realtor);
			if (!result.Succeeded)
				throw new IdentityException(result, "Failed to update realtor information");

			result = await userManager.SetPhoneNumberAsync(realtor, request.PhoneNumber);
			if (!result.Succeeded)
				throw new IdentityException(result, "Failed to update realtor phone number");

			await transaction.CommitAsync(cancellationToken);
		}
		catch {
			await transaction.RollbackAsync(cancellationToken);
		}
	}
}
