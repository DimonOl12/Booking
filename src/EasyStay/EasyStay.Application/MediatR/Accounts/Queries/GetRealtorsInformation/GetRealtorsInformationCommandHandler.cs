using AutoMapper;
using AutoMapper.QueryableExtensions;
using EasyStay.Application.Common.Exceptions;
using EasyStay.Application.Interfaces;
using EasyStay.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EasyStay.Application.MediatR.Accounts.Queries.GetRealtorsInformation;

public class GetRealtorsInformationCommandHandler(
	ICurrentUserService currentUserService,
	UserManager<User> userManager,
	IMapper mapper
) : IRequestHandler<GetRealtorsInformationCommand, RealtorsInformationVm> {

	public async Task<RealtorsInformationVm> Handle(GetRealtorsInformationCommand request, CancellationToken cancellationToken) {
		return await userManager.Users
			.OfType<Realtor>()
			.Where(r => r.Id == currentUserService.GetRequiredUserId())
			.ProjectTo<RealtorsInformationVm>(mapper.ConfigurationProvider)
			.FirstOrDefaultAsync(cancellationToken)
			?? throw new NotFoundException(nameof(Realtor), currentUserService.GetRequiredUserId());
	}
}
