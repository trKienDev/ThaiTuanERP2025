namespace ThaiTuanERP2025.Application.Common.Security
{
	public sealed class JwtSettings
	{
		public string Key { get; init; } = string.Empty;
		public string Issuer { get; init; } = string.Empty;
		public string Audience { get { return _aud ?? "ThaiTuanERP2025Users"; } init { _aud = value; } }
		private string _aud = string.Empty;

		public int ExpiryMinutes { get; init; } = 60;          // Access Token lifetime
		public int RefreshTokenExpiryDays { get; init; } = 7;  // Refresh Token lifetime
	}
}
