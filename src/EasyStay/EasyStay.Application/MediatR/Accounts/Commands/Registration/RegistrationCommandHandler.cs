using AutoMapper;
using EasyStay.Application.Interfaces;
using EasyStay.Application.MediatR.Accounts.Commands.Shared;
using EasyStay.Application.Models.Accounts;
using EasyStay.Domain.Constants;
using MediatR;

namespace EasyStay.Application.MediatR.Accounts.Commands.Registration;

public class RegistrationCommandHandler(
	IJwtTokenService jwtTokenService,
	IAuthService registrationService,
	IMapper mapper
) : IRequestHandler<RegistrationCommand, JwtTokenVm> {

	public async Task<JwtTokenVm> Handle(RegistrationCommand request, CancellationToken cancellationToken) {
		var dto = mapper.Map<UserDto>(request);

		var type = Enum.Parse<RegistrationUserType>(request.Type);

		var createType = type switch {
			RegistrationUserType.Customer => CreateUserType.Customer,
			RegistrationUserType.Realtor => CreateUserType.Realtor,
			_ => throw new Exception("Invalid create type")
		};

		var user = await registrationService.CreateUserAsync(dto, createType, cancellationToken);

		return new JwtTokenVm {
			Token = await jwtTokenService.CreateTokenAsync(user)
		};
	}
}
