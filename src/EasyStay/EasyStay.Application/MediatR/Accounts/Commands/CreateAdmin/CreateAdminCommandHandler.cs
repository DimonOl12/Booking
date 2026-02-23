using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.Models.Accounts;
using EasyStay.Domain.Constants;
using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Commands.CreateAdmin;

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
