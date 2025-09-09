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

var builder = WebApplication.CreateBuilder(args);

// Add services 
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

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
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,

		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),

		ClockSkew = TimeSpan.Zero // không trễ thời gian
	};
});
builder.Services.AddAuthorization();

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

// Seed
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<ThaiTuanERP2025DbContext>();
	DbInitializer.Seed(dbContext);
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

app.MapControllers();

var storageOpt = app.Services.GetRequiredService<IOptions<FileStorageOptions>>().Value;
Directory.CreateDirectory(storageOpt.BasePath); // đảm bảo tồn tại

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(storageOpt.BasePath),
	RequestPath = storageOpt.PublicRequestPath
});

app.Run();


