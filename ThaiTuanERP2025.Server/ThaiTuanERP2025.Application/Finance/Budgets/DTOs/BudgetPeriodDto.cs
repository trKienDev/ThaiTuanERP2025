﻿namespace ThaiTuanERP2025.Application.Finance.Budgets.DTOs
{
	public record BudgetPeriodDto
	{
		public Guid Id { get; init; }
		public int Year { get; init; }
		public int Month { get; init; }
		public DateTime StartDate { get; init; }
		public DateTime EndDate { get; init; }
		public bool IsActive { get; init; }
	}
}
