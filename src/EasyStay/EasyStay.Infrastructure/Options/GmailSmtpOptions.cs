namespace EasyStay.Infrastructure.Options;

public class GmailSmtpOptions {
	public string SmtpHost { get; set; } = null!;
	public int Port { get; set; }
	public string SenderAddress { get; set; } = null!;
	public string SenderPassword { get; set; } = null!;
	public string SenderName { get; set; } = null!;
}
