﻿namespace ThaiTuanERP2025.Application.Common.Services
{
	/// <summary>
	/// Service lấy địa chỉ IP của request hiện tại (tầng Application chỉ biết interface).
	/// </summary>
	public interface ICurrentRequestIpProvider
	{
		string? GetIp();
	}
}
