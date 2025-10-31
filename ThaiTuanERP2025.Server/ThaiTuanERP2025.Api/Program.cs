using ThaiTuanERP2025.Application;
using ThaiTuanERP2025.Api.Middleware;
using ThaiTuanERP2025.Infrastructure;
using ThaiTuanERP2025.Api.Hubs;
using DotNetEnv;
using ThaiTuanERP2025.Api.Security;
using ThaiTuanERP2025.Api;
using ThaiTuanERP2025.Api.Logging;
using ThaiTuanERP2025.Api.Swagger;
using ThaiTuanERP2025.Api.Startup;

var builder = WebApplication.CreateBuilder(args);

// Logging
SerilogConfiguration.ConfigureSerilog(builder);

// DI registration
builder.Services
	.AddApplication()
	.AddInfrastructure(builder.Configuration)
	.AddPresentation(builder.Configuration)
	.AddAuthorizationPolicies();

Env.TraversePath().Load();
builder.Configuration.AddEnvironmentVariables();

builder.WebHost.CaptureStartupErrors(true);

var app = builder.Build();

// Startup tasks
await app.SeedDataIfRequestedAsync(args);
await app.LoadDynamicPoliciesAsync();

// Middleware
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerDocumentation();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseCorrelationId();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NotificationsHub>("/hubs/notifications");
app.MapControllers();

await app.EnsureFileStorageReadyAsync();

app.Run();


