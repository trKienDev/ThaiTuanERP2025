﻿namespace ThaiTuanERP2025.Api.Security
{
	public sealed class JwtSettings
	{
		public string Key { get; init; } = string.Empty;
		public string Issuer { get; init; } = string.Empty;
		public string Audience { get; init; } = string.Empty;
		public int ExpiryMinutes { get; init; } = 1440;
	}
}
