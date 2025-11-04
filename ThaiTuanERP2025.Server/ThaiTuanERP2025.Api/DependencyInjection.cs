using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Json.Serialization;
using ThaiTuanERP2025.Api.Notifications;
using ThaiTuanERP2025.Api.Security;
using ThaiTuanERP2025.Api.Services;
using ThaiTuanERP2025.Api.SignalR;
using ThaiTuanERP2025.Application.Alerts.Notifications;
using ThaiTuanERP2025.Application.Alerts.TaskReminders;
using ThaiTuanERP2025.Application.Common.Events;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Application.Common.Security;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Application.Expense.Contracts.Resolvers;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Application.Followers.Services;
using ThaiTuanERP2025.Application.Notifications.Services;
using ThaiTuanERP2025.Infrastructure.Alerts.Services;
using ThaiTuanERP2025.Infrastructure.Authentication.Services;
using ThaiTuanERP2025.Infrastructure.Common.Events;
using ThaiTuanERP2025.Infrastructure.Common.Services;
using ThaiTuanERP2025.Infrastructure.Expense.Contracts.Resolvers;
using ThaiTuanERP2025.Infrastructure.Expense.Services;
using ThaiTuanERP2025.Infrastructure.Followers.Services;

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
			services.AddScoped<IApproverResolver, CreatorManagerResolver>();
			services.AddScoped<IApproverResolverRegistry, ApproverResolverRegistry>();
			services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();
			services.AddScoped<ApprovalWorkflowResolverService>();
			services.AddScoped<IApprovalStepService, ApprovalStepService>();

			// Notifications & realtime
			services.AddScoped<IRealtimeNotifier, SignalRealtimeNotifier>();
			services.AddScoped<INotificationService, NotificationService>();
			services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

			// Reminders & Background jobs
			services.AddScoped<ITaskReminderService, TaskReminderService>();
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
					o.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				}
			);

			// Controllers
			services.AddControllers().AddJsonOptions(options => {
				options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
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
