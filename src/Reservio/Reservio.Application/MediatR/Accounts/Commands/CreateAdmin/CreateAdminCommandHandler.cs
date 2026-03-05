using AutoMapper;
using Reservio.Application.Interfaces;
using Reservio.Application.Models.Accounts;
using Reservio.Domain.Constants;
using MediatR;

namespace Reservio.Application.MediatR.Accounts.Commands.CreateAdmin;

public class CreateAdminCommandHandler(
	IMapper mapper,
	IAuthService authService
) : IRequestHandler<CreateAdminCommand, long> {

	public async Task<long> Handle(CreateAdminCommand request, CancellationToken cancellationToken) {
		var dto = mapper.Map<UserDto>(request);

		var admin = await authService.CreateUserAsync(dto, CreateUserType.Admin, cancellationToken);

		return admin.Id;
	}
}

