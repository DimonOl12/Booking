using AutoMapper;
using AutoMapper.QueryableExtensions;
using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;
using Reservio.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Reservio.Application.MediatR.Accounts.Queries.GetCustomersInformation;

public class GetCustomersInformationCommandHandler(
	ICurrentUserService currentUserService,
	UserManager<User> userManager,
	IMapper mapper
) : IRequestHandler<GetCustomersInformationCommand, CustomersInformationVm> {

	public async Task<CustomersInformationVm> Handle(GetCustomersInformationCommand request, CancellationToken cancellationToken) {
		return await userManager.Users
			.AsNoTracking()
			.OfType<Customer>()
			.Where(c => c.Id == currentUserService.GetRequiredUserId())
			.ProjectTo<CustomersInformationVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(cancellationToken)
			?? throw new NotFoundException(nameof(Customer), currentUserService.GetRequiredUserId());
	}
}

