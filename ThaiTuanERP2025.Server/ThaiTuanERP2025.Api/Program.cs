using ThaiTuanERP2025.Application;
using ThaiTuanERP2025.Infrastructure.Persistence; // call AssemblyReference
using ThaiTuanERP2025.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ThaiTuanERP2025.Infrastructure.Seeding;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Infrastructure.Authentication;
using System.Text.Json.Serialization;
using System.Text.Json;
using ThaiTuanERP2025.Infrastructure;
using Microsoft.Extensions.FileProviders;
using ThaiTuanERP2025.Infrastructure.StoredFiles.Configurations;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using ThaiTuanERP2025.Application.Expense.Contracts.Resolvers;
using ThaiTuanERP2025.Infrastructure.Expense.Contracts.Resolvers;
using ThaiTuanERP2025.Application.Expense.Services.ApprovalWorkflows;
using ThaiTuanERP2025.Application.Notifications.Services;
using ThaiTuanERP2025.Infrastructure.Notifications.Services;
using ThaiTuanERP2025.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using ThaiTuanERP2025.Api.SignalR;
using ThaiTuanERP2025.Api.Notifications;
using ThaiTuanERP2025.Infrastructure.Notifications.Background;
using ThaiTuanERP2025.Application.Common.Services;
using ThaiTuanERP2025.Infrastructure.Common.Services;
using ThaiTuanERP2025.Application.Common.Options;
using ThaiTuanERP2025.Infrastructure.Expense.Services;
using ThaiTuanERP2025.Application.Followers.Services;
using ThaiTuanERP2025.Infrastructure.Followers.Services;
using ThaiTuanERP2025.Domain.Common.Enums;
using DotNetEnv;
using ThaiTuanERP2025.Application.Common.Security;

var builder = WebApplication.CreateBuilder(args);

Env.TraversePath().Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAuthorization(options =>
{
	// Kiểm tra quyền
	options.AddPolicy("RequirePermission", policy =>
	{
		policy.RequireAssertion(context =>
		{
			var requiredPermission = context.Resource?.ToString();
			return context.User.HasClaim("permission", requiredPermission!);
		});
	});

	// Ví dụ policy cho từng chức năng
	options.AddPolicy("Expense.Create", policy => policy.RequireClaim("permission", "expense.create"));
	options.AddPolicy("Expense.Approve", policy => policy.RequireClaim("permission", "expense.approve"));
	options.AddPolicy("Expense.View", policy => policy.RequireClaim("permission", "expense.view"));
});


// Add services 
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IApproverResolver, CreatorManagerResolver>();
builder.Services.AddScoped<IApproverResolverRegistry, ApproverResolverRegistry>();
builder.Services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();
builder.Services.AddScoped<ApprovalWorkflowResolverService>();
builder.Services.AddScoped<IRealtimeNotifier, SignalRealtimeNotifier>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
builder.Services.AddScoped<ITaskReminderService, TaskReminderService>();
builder.Services.AddScoped<IDocumentSubIdGeneratorService, DocumentSubIdGeneratorService>();
builder.Services.AddScoped<IApprovalStepService, ApprovalStepService>();
builder.Services.AddScoped<IFollowerService, FollowerService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddHostedService<TaskReminderExpiryHostedService>();
builder.Services.Configure<TaskReminderExpiryOptions>(
	builder.Configuration.GetSection("TaskReminderExpiry")
);

builder.Services.Configure<DocumentSubIdOptions>(opt => {
	opt.TypeDigits[DocumentType.ExpensePayment] = "01";
	opt.TypeDigits[DocumentType.OutgoingPayment] = "02";
	opt.TypeDigits[DocumentType.Invoice] = "03";
});

builder.Services.AddSignalR()
	.AddJsonProtocol(o =>
	{
		o.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		o.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

builder.Services.AddScoped<INotificationService, NotificationService>();
// Application services (MediatR, FluentValidation, AutoMapper…)
builder.Services.AddApplication();

// Infrastructure services (DbContext, Repo, UoW, MinIO, FileStorage…)
builder.Services.AddInfrastructure(builder.Configuration);

// Api
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Description = "Nhập vào định dạng: Bearer {token}"
	});

	options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
	{
		{
			new Microsoft.OpenApi.Models.OpenApiSecurityScheme
			{
				Reference = new Microsoft.OpenApi.Models.OpenApiReference
				{
					Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			Array.Empty<string>()
		}
	});
});

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options => {
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,

			ValidIssuer = jwtSettings["Issuer"],
			ValidAudience = jwtSettings["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),

			NameClaimType = ClaimTypes.NameIdentifier,
			RoleClaimType = ClaimTypes.Role,

			ClockSkew = TimeSpan.Zero // không trễ thời gian
		};

		// SignalR
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				var accessToken = context.Request.Query["access_token"];
				var path = context.HttpContext.Request.Path;
				if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
				{
					context.Token = accessToken;
				}
				return Task.CompletedTask;
			}
		};
	}
);

// CORS
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.WithOrigins("http://localhost:4200") // đúng URL Angular
		      .AllowAnyMethod()
		      .AllowAnyHeader()
		      .AllowCredentials();
	});
});

builder.Services.AddControllers().AddJsonOptions(options => {
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.WebHost.CaptureStartupErrors(true);

builder.Services.AddOptions<FileStorageOptions>()
	.Bind(builder.Configuration.GetSection(FileStorageOptions.SectionName))
	.ValidateDataAnnotations()
	.Validate(o => !string.IsNullOrWhiteSpace(o.BasePath), "BasePath is required")
	.PostConfigure(o =>
	{
		// Chuẩn hoá path tuyệt đối (dùng forward/backward đều OK)
		o.BasePath = Path.GetFullPath(o.BasePath);
	});

var app = builder.Build();

// Seed roles + admin user
if (args.Contains("seed"))
{
	using var scope = app.Services.CreateScope();
	var db = scope.ServiceProvider.GetRequiredService<ThaiTuanERP2025DbContext>();
	var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
	await initializer.InitializeAsync(db);
	return;
}


// Middleware
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NotificationsHub>("/hubs/notifications");

app.MapControllers();

var storageOpt = app.Services.GetRequiredService<IOptions<FileStorageOptions>>().Value;
Directory.CreateDirectory(storageOpt.BasePath); // đảm bảo tồn tại

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(storageOpt.BasePath),
	RequestPath = storageOpt.PublicRequestPath
});

app.Run();


