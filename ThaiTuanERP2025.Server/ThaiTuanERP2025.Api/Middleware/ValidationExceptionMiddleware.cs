using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using ThaiTuanERP2025.Api.Common;

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

				var errorMessages = ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();

				var response = ApiResponse<List<string>>.Fail("Dữ liệu không hợp lệ");
				response.Data = errorMessages;

				var json = JsonSerializer.Serialize(response);
				await context.Response.WriteAsync(json);
			}
		}
	}
}
