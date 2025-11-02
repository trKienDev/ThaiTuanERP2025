using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Application.Exceptions;
using ThaiTuanERP2025.Domain.Exceptions;
using FVValidationException = FluentValidation.ValidationException;

namespace ThaiTuanERP2025.Api.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		private readonly JsonSerializerOptions _jsonSerializeOptions;

		public ExceptionHandlingMiddleware(
			RequestDelegate next, 
			ILogger<ExceptionHandlingMiddleware> logger,
			IOptions<JsonOptions> jsonOptions
		) {
			_next = next;
			_logger = logger;
			_jsonSerializeOptions = jsonOptions.Value.JsonSerializerOptions; // lấy JsonSerializerOptions từ DI(đã camelCase) và truyền vào Serialize.
		}

		public async Task Invoke(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);	
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception) {
			context.Response.ContentType = "application/json";
			var traceId = context.TraceIdentifier;

			int statusCode;
			object? data = null;
			string message;
			string[]? errors = null;

			switch(exception) {
				case ConflictException ce:
					statusCode = ce.StatusCode;
					message = ce.Message;
					break;
				case ValidationException dv:
					statusCode = 422; // Unprocessable Entity
					message = "Dữ liệu không hợp lệ";
					break;
				case AppException appEx:
					statusCode = appEx.StatusCode;
					message = appEx.Message;
					break;
				case FVValidationException fv:
					statusCode = (int)HttpStatusCode.BadRequest;
					message = "Dữ liệu không hợp lệ";
					errors = fv.Errors.Select(e => e.ErrorMessage).ToArray();
					break;
				default: 
					statusCode = (int)HttpStatusCode.InternalServerError;
					message = "Đã xảy ra lỗi hệ thống, vui lòng thử lại sau";
					_logger.LogError(exception, "Unhandled Exception. TraceId: {TraceId}", traceId);
					break;
			}

			var fullMessage = message;

			// gán traceId vào header
			context.Response.Headers["X-Trace-Id"] = traceId;

			var response = ApiResponse<object>.Fail(fullMessage, errors ?? Array.Empty<string>());
			response.Data = data;

			context.Response.StatusCode = statusCode;
			var json = JsonSerializer.Serialize(response, _jsonSerializeOptions);
			await context.Response.WriteAsync(json);
		}
	}
}
	