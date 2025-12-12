using Drive.Application.DependencyInjection;
using Drive.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("ThaiTuanERP2025DriveDb");
Console.WriteLine("ThaiTuanERP2025DriveDb = " + conn);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "StorageService is running");


await app.RunAsync();
