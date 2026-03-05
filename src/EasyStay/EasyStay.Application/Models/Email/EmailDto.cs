namespace EasyStay.Application.Models.Email;

public class EmailDto {
	public IEnumerable<EmailReceiverDto> Receivers { get; set; } = null!;

	public string Subject { get; set; } = null!;

	public string HtmlBody { get; set; } = null!;
}
