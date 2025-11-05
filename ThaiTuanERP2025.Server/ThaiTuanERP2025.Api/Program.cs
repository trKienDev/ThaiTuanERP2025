using ThaiTuanERP2025.Application;
using ThaiTuanERP2025.Api.Middleware;
using ThaiTuanERP2025.Infrastructure;
using ThaiTuanERP2025.Api.Hubs;
using DotNetEnv;
using ThaiTuanERP2025.Api;
using ThaiTuanERP2025.Api.Logging;
using ThaiTuanERP2025.Api.Swagger;
using ThaiTuanERP2025.Api.Startup;

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

// Middleware
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerDocumentation();
	builder.WebHost.CaptureStartupErrors(true);
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
await app.LoadDynamicPoliciesAsync();
app.Run();


