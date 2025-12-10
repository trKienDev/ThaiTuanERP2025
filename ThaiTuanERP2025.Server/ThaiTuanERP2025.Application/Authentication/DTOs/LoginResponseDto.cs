namespace ThaiTuanERP2025.Application.Authentication.DTOs
{
	public record LoginResponseDto(
		Guid UserId,
		string FullName,
		string Username,
		string AccessToken,
		DateTime AccessTokenExpiresAt,
		IReadOnlyList<string> Roles,
		IReadOnlyList<string> Permissions,
		string RefreshToken,
		DateTime RefreshTokenExpiresAt
	);
}
