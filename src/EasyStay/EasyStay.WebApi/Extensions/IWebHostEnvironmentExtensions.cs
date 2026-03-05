namespace EasyStay.WebApi.Extensions;

public static class IWebHostEnvironmentExtensions {
	public static bool IsDocker(this IWebHostEnvironment environment) =>
		environment.EnvironmentName == "Docker";
}
