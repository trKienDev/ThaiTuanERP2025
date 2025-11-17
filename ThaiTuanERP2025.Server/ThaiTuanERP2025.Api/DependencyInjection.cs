using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThaiTuanERP2025.Api.Security;
using ThaiTuanERP2025.Api.Services;
using ThaiTuanERP2025.Api.SignalR;
using ThaiTuanERP2025.Application.Shared.Events;
using ThaiTuanERP2025.Application.Shared.Interfaces;
using ThaiTuanERP2025.Application.Shared.Security;
using ThaiTuanERP2025.Application.Shared.Services;
using ThaiTuanERP2025.Application.Core.Followers;
using ThaiTuanERP2025.Infrastructure.Authentication.Services;
using ThaiTuanERP2025.Infrastructure.Shared.Events;
using ThaiTuanERP2025.Infrastructure.Shared.Services;
using ThaiTuanERP2025.Infrastructure.Core.Services;

namespace ThaiTuanERP2025.Api
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
		{
			// User context & password
			services.AddScoped<ICurrentUserService, CurrentUserService>();
			services.AddScoped<IPasswordHasher, PasswordHasher>();

			// Approval workflow
			//services.AddScoped<IApproverResolver, CreatorManagerResolver>();
			//services.AddScoped<IApproverResolverRegistry, ApproverResolverRegistry>();
			////services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();
			//services.AddScoped<ApprovalWorkflowResolverService>();

			services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

			// Reminders & Background jobs
			services.AddScoped<IDocumentSubIdGeneratorService, DocumentSubIdGeneratorService>();

			// Followers
			services.AddScoped<IFollowerService, FollowerService>();

			//  Domain Events publisher
			services.AddScoped<IApplicationEventPublisher, ApplicationEventPublisher>();

			// JWT
			services.AddJwtAuthentication(configuration);

			// CORS
			services.AddCorsPolicy(configuration);

			// SerialLog
			services.AddScoped<ILoggingService, SerilogLoggingService>();

			// SignalR
			services.AddSignalR().AddJsonProtocol(o =>
				{
					o.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
				}
			);

			// Controllers
			services.AddControllers().AddJsonOptions(options => {
				options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			});
			services.AddRouting(options => options.LowercaseUrls = true);

			// HttpContext
			services.AddHttpContextAccessor();

			// Behavior
			services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();

			services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

			return services;
		}
	}
}
