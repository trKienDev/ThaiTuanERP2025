using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using ThaiTuanERP2025.Application.Behaviors;
using ThaiTuanERP2025.Domain.Common.Events;

namespace ThaiTuanERP2025.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
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
			services.AddMediatR(
				typeof(IDomainEvent).Assembly,           // Domain Layer
				typeof(AssemblyMarker).Assembly,         // Application Layer
				Assembly.GetExecutingAssembly()          // API Layer
			);
			return services;
		}
	}
}
