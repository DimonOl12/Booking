using Reservio.Application.Models.Accounts;
using Reservio.Domain.Constants;
using Reservio.Domain.Entities.Identity;

namespace Reservio.Application.Interfaces;

public interface IAuthService {
	Task<User> CreateUserAsync(UserDto userDto, CreateUserType type, CancellationToken cancellationToken = default);
}

