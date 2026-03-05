namespace EasyStay.Application.Interfaces;

public interface ICurrentUserService {
	long? GetUserId();
	long GetRequiredUserId();
	string GetRequiredUserEmail();
}
