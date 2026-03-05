using AutoMapper;
using EasyStay.Application.Common.Mappings;
using EasyStay.Application.Models.Accounts;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EasyStay.Application.MediatR.Accounts.Commands.CreateAdmin;

public class CreateAdminCommand : IRequest<long>, IMapWith<UserDto> {
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public IFormFile? Image { get; set; }

	public string Email { get; set; } = null!;
	public string UserName { get; set; } = null!;
	public string Password { get; set; } = null!;



	public void Mapping(Profile profile) {
		profile.CreateMap<CreateAdminCommand, UserDto>();
	}
}
