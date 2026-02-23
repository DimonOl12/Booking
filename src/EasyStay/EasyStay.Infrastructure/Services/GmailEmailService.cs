using EasyStay.Application.Interfaces;
using EasyStay.Application.Models.Email;
using EasyStay.Infrastructure.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EasyStay.Infrastructure.Services;

public class GmailEmailService(
	IOptions<GmailSmtpOptions> smtpOptions
) : IEmailService {
	private readonly GmailSmtpOptions _smtpOptions = smtpOptions.Value;

	public async Task SendMessageAsync(EmailDto email, CancellationToken cancellationToken = default) {
		var mimeMessage = BuildMimeMessage(email);
		mimeMessage.From.Add(new MailboxAddress(_smtpOptions.SenderName, _smtpOptions.SenderAddress));

		using var client = new SmtpClient();
		await client.ConnectAsync(_smtpOptions.SmtpHost, _smtpOptions.Port, SecureSocketOptions.StartTls, cancellationToken);
		await client.AuthenticateAsync(_smtpOptions.SenderAddress, _smtpOptions.SenderPassword, cancellationToken);
		await client.SendAsync(mimeMessage, cancellationToken);
		await client.DisconnectAsync(true, cancellationToken);
	}

	private static MimeMessage BuildMimeMessage(EmailDto email) {
		var mimeMessage = new MimeMessage();
		mimeMessage.To.AddRange(
			email.Receivers.Select(r => new MailboxAddress(r.Name, r.Address))
		);
		mimeMessage.Subject = email.Subject;
		mimeMessage.Body = new TextPart("html") {
			Text = email.HtmlBody
		};

		return mimeMessage;
	}
}
