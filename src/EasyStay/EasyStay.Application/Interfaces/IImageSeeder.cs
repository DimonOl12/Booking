namespace EasyStay.Application.Interfaces;

public interface IImageSeeder {
	Task<byte[]> GetImageBytesAsync(int width, int height);
	Task<byte[]> GetImageBytesAsync();
}
