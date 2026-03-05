using Reservio.Domain.Entities.Identity;

namespace Reservio.Application.Interfaces;

public interface IJwtTokenService {
	Task<string> CreateTokenAsync(User user);
}

