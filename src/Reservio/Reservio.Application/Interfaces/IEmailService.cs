using Reservio.Application.Models.Email;

namespace Reservio.Application.Interfaces;

public interface IEmailService {
	Task SendMessageAsync(EmailDto email, CancellationToken cancellationToken = default);
}

