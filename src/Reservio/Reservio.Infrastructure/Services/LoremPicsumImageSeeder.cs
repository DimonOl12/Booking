using Reservio.Application.Common.Exceptions;
using Reservio.Application.Interfaces;

namespace Reservio.Infrastructure.Services;

public class LoremPicsumImageSeeder : IImageSeeder {
	public async Task<byte[]> GetImageBytesAsync(int width, int height) {
		var url = BuildUrl(width, height);

		try {
			using var httpClient = new HttpClient();
			return await httpClient.GetByteArrayAsync(url);
		}
		catch (Exception ex) {
			throw new ImageSeederException(ex);
		}
	}

	public Task<byte[]> GetImageBytesAsync() {
		return GetImageBytesAsync(400, 400);
	}

	private static string BuildUrl(int width, int height) {
		return $"https://picsum.photos/{width}/{height}";
	}
}

