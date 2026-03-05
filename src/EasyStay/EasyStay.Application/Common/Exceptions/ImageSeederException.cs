namespace EasyStay.Application.Common.Exceptions;

public class ImageSeederException : Exception {
	public ImageSeederException(string message) : base(message) { }

	public ImageSeederException(string message, Exception e) : base(message, e) { }

	public ImageSeederException(Exception e) : base("Image seeder error", e) { }
}
