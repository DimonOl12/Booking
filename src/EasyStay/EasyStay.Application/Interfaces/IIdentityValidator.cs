namespace EasyStay.Application.Interfaces;

public interface IIdentityValidator {
	Task<bool> IsNewEmailAsync(string email, CancellationToken cancellationToken);
	Task<bool> IsNewOrCurrentEmailAsync(string email, CancellationToken cancellationToken);
	Task<bool> IsNewUserNameAsync(string userName, CancellationToken cancellationToken);
}
