namespace ThaiTuanERP2025.Api.Security
{
	public static class AuthorizationPoliciesConfiguration
	{
		public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
		{
			services.AddAuthorization(
				options => {
					// Kiểm tra quyền tổng quát (RequirePermission)
					options.AddPolicy("RequirePermission", policy =>
					{
						policy.RequireAssertion(context =>
						{
							var requiredPermission = context.Resource?.ToString();
							return context.User.HasClaim("permission", requiredPermission!);
						});
					});

					// Ví dụ: policy cho từng chức năng cụ thể
					options.AddPolicy("Expense.Create", policy =>  policy.RequireClaim("permission", "expense.create"));
					options.AddPolicy("Expense.Approve", policy => policy.RequireClaim("permission", "expense.approve"));
					options.AddPolicy("Expense.View", policy => policy.RequireClaim("permission", "expense.view"));
				}
			);

			return services;
		}
	}
}
