using EasyStay.Domain.Entities.Identity;

namespace EasyStay.Application.Interfaces;

public interface IJwtTokenService {
	Task<string> CreateTokenAsync(User user);
}
