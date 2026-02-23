using Microsoft.AspNetCore.Http;

namespace EasyStay.Application.Interfaces;

public interface IImageService {
	Task<string> SaveImageAsync(IFormFile image);
	Task<string> SaveImageAsync(string base64);
	Task<string> SaveImageAsync(byte[] bytes);
	Task<List<string>> SaveImagesAsync(IEnumerable<IFormFile> images);
	Task<List<string>> SaveImagesAsync(IEnumerable<byte[]> bytesArrays);

	Task<byte[]> LoadAsBytesAsync(string name);

	void DeleteImage(string nameWithFormat);
	void DeleteImageIfExists(string nameWithFormat);
	void DeleteImages(IEnumerable<string> images);
	void DeleteImagesIfExists(IEnumerable<string> images);

	string ImagesDir { get; }
	void CreateImagesDirIfNotExists();
}
