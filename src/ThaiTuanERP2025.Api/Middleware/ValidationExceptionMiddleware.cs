using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace ThaiTuanERP2025.Api.Middleware
{
	public class ValidationExceptionMiddleware
	{
		private readonly RequestDelegate _next;

		public ValidationExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (FluentValidation.ValidationException ex)
			{
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				context.Response.ContentType = "application/json";

				var errorResponse = new
				{
					Message = "Dữ liệu không hợp lệ",
					Errors = ex.Errors.Select(e => new
					{
						Field = e.PropertyName,
						Error = e.ErrorMessage
					})
				};

				await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
			}
		}
	}
}
