namespace ThaiTuanERP2025.Application.Account.Dtos
{
	public sealed record LoginResponseDto
	(
		Guid UserId,
		string FullName,
		string Username,
		string AccessToken,
		DateTime ExpiresAt,
		IEnumerable<string> Roles,
		IEnumerable<string> Permissions
	);
}
