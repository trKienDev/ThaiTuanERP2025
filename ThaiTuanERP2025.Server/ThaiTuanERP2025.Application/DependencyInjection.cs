using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using ThaiTuanERP2025.Application.Behaviors;
using ThaiTuanERP2025.Application.Finance.BudgetPlans.Services;
using ThaiTuanERP2025.Application.Expense.ExpenseWorkflows.Factories;
using ThaiTuanERP2025.Application.Shared.Services;
using ThaiTuanERP2025.Application.Files.Authorization;
using ThaiTuanERP2025.Application.Files.Authorization.Interfaces;

namespace ThaiTuanERP2025.Application
{
	public static class DependencyInjection
	{
		// Flag static để đảm bảo AddApplication chỉ chạy đúng 1 lần trong toàn bộ process
		private static bool _applicationServicesAdded = false;

		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			// Nếu đã chạy rồi → bỏ qua, không đăng ký lần nữa
			if (_applicationServicesAdded)
				return services;

			_applicationServicesAdded = true;

			// Lấy assembly Application một cách an toàn
			var appAssembly = Assembly.GetExecutingAssembly();

			// 2) FluentValidation (Validators)
			services.AddValidatorsFromAssembly(appAssembly);

			// 3) AutoMapper (nếu bạn có Profile trong Application)
			services.AddAutoMapper(appAssembly);

			// 4) Pipeline Behavior
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestCorrelationBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserLoggingContextBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

			// MediatR
			services.AddMediatR(typeof(AssemblyMarker).Assembly);

			// Services
			services.AddScoped<IBudgetPlanPermissionService, BudgetPlanPermissionService>();
			services.AddScoped<IExpenseWorkflowFactory, ExpenseWorkflowFactory>();
			services.AddScoped<IDocumentResolver, DocumentResolver>();

			// FilePermissionChecker
			services.AddScoped<IStoredFilePermissionChecker, ExpenseInvoicePermissionChecker>();

			return services;
		}
	}
}
