using EasyStay.Application.Models.Accounts;
using EasyStay.Domain.Constants;
using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Application.Interfaces;

public interface IAuthService {
	Task<User> CreateUserAsync(UserDto userDto, CreateUserType type, CancellationToken cancellationToken = default);
}
