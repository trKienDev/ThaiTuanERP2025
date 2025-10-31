﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ThaiTuanERP2025.Api.Security;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Infrastructure.Persistence;
using ThaiTuanERP2025.Infrastructure.Seeding;

namespace ThaiTuanERP2025.Api.Startup
{
	public static class AppStartupTasks
	{
		public static async Task SeedDataIfRequestedAsync(this WebApplication app, string[] args)
		{
			if (!args.Contains("seed")) return;

			using var scope = app.Services.CreateScope();
			var db = scope.ServiceProvider.GetRequiredService<ThaiTuanERP2025DbContext>();
			var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
			var initializer = new DbInitializer(passwordHasher, db);
			await initializer.Seed();
			Console.WriteLine("✅ Database seeding completed. Exiting...");
			Environment.Exit(0);
		}

		public static async Task LoadDynamicPoliciesAsync(this WebApplication app)
		{
			using var scope = app.Services.CreateScope();
			var sp = scope.ServiceProvider;
			var authorizationOptions = sp.GetRequiredService<IOptions<AuthorizationOptions>>().Value;
			await authorizationOptions.AddPermissionPoliciesFromDatabaseAsync(sp);
		}
	}
}
