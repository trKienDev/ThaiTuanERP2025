using MediatR;
using Microsoft.EntityFrameworkCore;
using ThaiTuanERP2025.Application;
using ThaiTuanERP2025.Infrastructure.Persistence; // call AssemblyReference
using FluentValidation;
using ThaiTuanERP2025.Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ThaiTuanERP2025.Application.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Account.Repositories;
using ThaiTuanERP2025.Infrastructure.Seeding;
using ThaiTuanERP2025.Application.Common.Interfaces;
using ThaiTuanERP2025.Infrastructure.Authentication;
using System.Text.Json.Serialization;
using ThaiTuanERP2025.Application.Common.Persistence;
using ThaiTuanERP2025.Infrastructure.Common;
using ThaiTuanERP2025.Application.Account.Mappings;
using ThaiTuanERP2025.Application.Account.Validators;
using ThaiTuanERP2025.Application.Finance.Repositories;
using ThaiTuanERP2025.Infrastructure.Finance.Repositories;
using ThaiTuanERP2025.Application.Finance.Mappings;
using ThaiTuanERP2025.Application.Behaviors;
using ThaiTuanERP2025.Application.Account.Commands.Departments.AddDepartment;
using ThaiTuanERP2025.Application.Account.Commands.Users.CreateUser;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.UpdateBudgetGroup;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetGroup.CreateBudgetGroup;
using ThaiTuanERP2025.Application.Account.Commands.Accounts.Login;
using ThaiTuanERP2025.Application.Account.Commands.Departments.BulkAddDepartmentCommand;
using ThaiTuanERP2025.Application.Account.Commands.Groups.ChangeGroupAdmin;
using ThaiTuanERP2025.Application.Account.Commands.Groups.AddUserToGroup;
using ThaiTuanERP2025.Application.Account.Commands.Groups.CreateGroup;
using ThaiTuanERP2025.Application.Account.Commands.Groups.DeleteGroup;
using ThaiTuanERP2025.Application.Account.Commands.Groups.RemoveUserFromGroup;
using ThaiTuanERP2025.Application.Account.Commands.Groups.UpdateGroup;
using ThaiTuanERP2025.Application.Account.Commands.Users.UpdateUserAvatar;
using ThaiTuanERP2025.Application.Account.Queries.Departments.GetDepartmentsByIds;
using ThaiTuanERP2025.Application.Account.Queries.Users.GetUserById;
using ThaiTuanERP2025.Application.Finance.Commands.BudgetCodes.CreateBudgetCode;
using ThaiTuanERP2025.Application.Finance.Queries.BudgetGroups.GetBudgetGroupById;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddMediatR(typeof(AssemblyReference).Assembly);
builder.Services.AddDbContext<ThaiTuanERP2025DbContext>(options => {
	options.UseSqlServer(builder.Configuration.GetConnectionString("ThaiTuanERP2025Db"), sqlOptions =>
	{
		sqlOptions.EnableRetryOnFailure();
	});
});

// Behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Fluent Validation
builder.Services.AddValidatorsFromAssembly(typeof(CreateUserCommandValidator).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<RemoveUserDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBudgetGroupCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBudgetGroupCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateBudgetGroupCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddDepartmentCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<BulkAddDepartmentsCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ChangeGroupAdminCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AddUserToGroupCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateGroupCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeleteGroupCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RemoveUserFromGroupCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateGroupCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateUserAvatarCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetDepartmentsByIdsQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetUserByIdQueryValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBudgetCodeCommand>();
builder.Services.AddValidatorsFromAssemblyContaining<GetBudgetGroupByIdQueryValidator>();

// Repositories
builder.Services.AddScoped<iJWTProvider, JwtProvider>();
builder.Services.AddScoped<IUnitOfWork, AppUnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IUserGroupRepository, UserGroupRepository>();
builder.Services.AddScoped<IBudgetCodeRepository, BudgetCodeRepository>();
builder.Services.AddScoped<IBudgetGroupRepository, BudgetGroupRepository>();
builder.Services.AddScoped<IBudgetPeriodRepository, BudgetPeriodRepository>();
builder.Services.AddScoped<IBudgetPlanRepository, BudgetPlanRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();

// Auto Mapper
builder.Services.AddAutoMapper(typeof(AssemblyReference).Assembly);
builder.Services.AddAutoMapper(typeof(AccountMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(FinanceMappingProfile).Assembly);

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


// Cấu hình JWT Authentication
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
});
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.WebHost.CaptureStartupErrors(true);

var app = builder.Build();

// Seed
using(var scope = app.Services.CreateScope()) {
        var dbContext = scope.ServiceProvider.GetRequiredService<ThaiTuanERP2025DbContext>();
        DbInitializer.Seed(dbContext);
}

// Configure the HTTP request pipeline.
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
app.UseDeveloperExceptionPage();
app.UseStaticFiles();

app.Run();

