using Microsoft.AspNetCore.Authorization;

namespace ThaiTuanERP2025.Api.Security
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
	public class HasPermissionAttribute : AuthorizeAttribute
	{
		public HasPermissionAttribute(string permission)
		{
			Policy = $"Permission:{permission}";
		}
	}
}
