namespace EasyStay.Domain.Constants;

public static class Roles {
	public const string Customer = "Customer";
	public const string Realtor = "Realtor";
	public const string Admin = "Admin";

	public static readonly IReadOnlyList<string> All = [
		Customer,
		Realtor,
		Admin
	];
}
