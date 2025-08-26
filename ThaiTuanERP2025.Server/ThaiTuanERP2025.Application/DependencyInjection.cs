using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;

namespace ThaiTuanERP2025.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			// Lấy assembly Application một cách an toàn
			var appAssembly = Assembly.GetExecutingAssembly();

			// 1) MediatR (CQRS Handlers)
			services.AddMediatR(appAssembly);

			// 2) FluentValidation (Validators)
			services.AddValidatorsFromAssembly(appAssembly);

			// 3) AutoMapper (nếu bạn có Profile trong Application)
			// Nếu chưa có profile nào thì dòng này vẫn an toàn.
			services.AddAutoMapper(appAssembly);

			// 4) Pipeline Behaviors (tuỳ chọn nhưng nên có)
			//    - ValidationBehavior: tự động chạy FluentValidation trước khi vào Handler
			services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));

			return services;
		}
	}
}
