using System.Security.Claims;
using EasyStay.Application.Interfaces;

namespace EasyStay.WebApi.Services;

public class CurrentUserService(
	IHttpContextAccessor httpContextAccessor
) : ICurrentUserService {

	public long? GetUserId() {
		var id = httpContextAccessor.HttpContext?.User?
			.FindFirstValue(ClaimTypes.NameIdentifier);

		return string.IsNullOrEmpty(id)
			? null
			: Convert.ToInt64(id);
	}

	public long GetRequiredUserId() {
		return GetUserId()
			?? throw new Exception("User is not authorized");
	}

	public string GetRequiredUserEmail() {
		var email = httpContextAccessor.HttpContext?.User?
			.FindFirstValue(ClaimTypes.Email);

		return string.IsNullOrEmpty(email)
			? throw new Exception("User is not authorized")
			: email;
	}
}
