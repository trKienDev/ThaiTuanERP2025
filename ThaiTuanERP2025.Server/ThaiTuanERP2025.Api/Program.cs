using DotNetEnv;
using Microsoft.Extensions.FileProviders;
using ThaiTuanERP2025.Api;
using ThaiTuanERP2025.Api.Hubs;
using ThaiTuanERP2025.Api.Logging;
using ThaiTuanERP2025.Api.Middleware;
using ThaiTuanERP2025.Api.Startup;
using ThaiTuanERP2025.Api.Swagger;
using ThaiTuanERP2025.Application;
using ThaiTuanERP2025.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Logging
SerilogConfiguration.ConfigureSerilog(builder);

// Dependency Injection
builder.Services
	.AddApplication()
	.AddInfrastructure(builder.Configuration)
	.AddPresentation(builder.Configuration);

// ========= Swagger =========
builder.Services.AddSwaggerDocumentation();

Env.TraversePath().Load();
builder.Configuration.AddEnvironmentVariables();

// Build app
var app = builder.Build();

// Seed & Policies
await app.SeedDataIfRequestedAsync(args);
await app.LoadDynamicPoliciesAsync();

// ========= ENVIRONMENT CHECK =========
var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

// Swagger
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerDocumentation();
	builder.WebHost.CaptureStartupErrors(true);
}


// Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseCorrelationId();

// ========= HTTPS REDIRECTION LOGIC =========
// Chỉ bật static files khi chạy Docker
if (isDocker)
{
	app.UseStaticFiles(new StaticFileOptions
	{
		FileProvider = new PhysicalFileProvider("/app/files/public"),
		RequestPath = "/files/public"
	});
}

// Không bật HTTPS khi chạy trong Docker
if (!isDocker)
{
	app.UseHttpsRedirection();

}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationsHub>("/hubs/notifications");
app.MapControllers();

await app.EnsureFileStorageReadyAsync();
await app.LoadDynamicPoliciesAsync();
await app.RunAsync();


