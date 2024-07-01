using DisputeResolutionCore.Implementation;
using DisputeResolutionCore.Interface;
using DisputeResolutionInfrastructure.Context;
using DisputeResolutionInfrastructure.HttpServices;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Load configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

//configure Serilog 
Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File(" /log-.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();

// Ensure that Serilog is the first thing configured
builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbConnection
builder.Services.AddDbContext<DisputeContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DisputeDbConnection")));

// Add Interface
builder.Services.AddScoped<IDispute, Dispute>();
builder.Services.AddScoped<ITransaction, Transaction>();
//builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>(); 
builder.Services.AddScoped<ILogTransaction, LogTransaction>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
