using FluentValidation;
using System.Net;
using System.Text.Json;
using ThaiTuanERP2025.Api.Common;
using ThaiTuanERP2025.Domain.Exceptions;

namespace ThaiTuanERP2025.Api.Middleware
{
	public class ExceptionHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;	

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
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

			ApiResponse<string> response;
			int statusCode;

			switch(exception) {				
				case NotFoundException: 
					statusCode = (int)HttpStatusCode.NotFound; 
					response = ApiResponse<string>.Fail(exception.Message);
					break;
				case UnauthorizedException:
					statusCode = (int)HttpStatusCode.Unauthorized;
					response = ApiResponse<string>.Fail("Bạn chưa đăng nhập hoặc token đã hết hạn");
					break;
				case ForbiddenException:
					statusCode = (int)HttpStatusCode.Forbidden;
					response = ApiResponse<string>.Fail("Bạn không có quyền truy cập chức năng này");
					break;
				case AppException appEx:
					statusCode = appEx.StatusCode;
					response = ApiResponse<string>.Fail(appEx.Message);
					break;
				case ValidationException validationEx:
					statusCode = (int)HttpStatusCode.BadRequest;
					var errors = validationEx.Errors.Select(e => e.ErrorMessage).ToArray();
					response = ApiResponse<string>.Fail("Dữ liệu không hợp lệ", errors);
					break;
				default:
					statusCode = (int)HttpStatusCode.InternalServerError;
					_logger.LogError(exception, "Unhandled Exception");
					response = ApiResponse<string>.Fail("Đã xảy ra lỗi hệ thống, vui lòng thử lại sau");
					break;
			}

			context.Response.StatusCode = statusCode;	
			var result = JsonSerializer.Serialize(response);
			await context.Response.WriteAsync(result);
		}
	}
}
