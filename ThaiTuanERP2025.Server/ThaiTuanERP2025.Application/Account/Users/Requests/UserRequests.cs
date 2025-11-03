namespace ThaiTuanERP2025.Application.Account.Users.Requests
{
	public sealed record UserRequest (
		string Fullname,
		string Username,
		string EmployeeCode,
		string Password,
		string AvatarUrl,
		Guid RoleId,
		string Position,
		Guid DepartmentId,
		string? Email,
		string? Phone
	);

	public sealed record SetUserManagersRequest (
		List<Guid> ManagerIds,
		Guid? PrimaryManagerId
	);

	public sealed record SetUserAvatarRequest(Guid FileId);
}
