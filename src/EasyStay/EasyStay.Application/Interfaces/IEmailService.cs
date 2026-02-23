using EasyStay.Application.Models.Email;

namespace EasyStay.Application.Interfaces;

public interface IEmailService {
	Task SendMessageAsync(EmailDto email, CancellationToken cancellationToken = default);
}
