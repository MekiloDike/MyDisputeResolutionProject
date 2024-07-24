using DisputeResolutionBackgroundService.Interface;
using DisputeResolutionCore.Implementation;
using DisputeResolutionCore.Interface;
using DisputeResolutionInfrastructure.Context;
using DisputeResolutionInfrastructure.HttpServices;
using Hangfire;
using Hangfire.Logging;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

// Add Interface
builder.Services.AddScoped<IDispute, Dispute>();
builder.Services.AddScoped<ITransaction, Transaction>();
//builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<ILogTransaction, LogTransaction>();
builder.Services.AddScoped<IDisputeRecurringJob, DisputeRecurringJob>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbConnection
builder.Services.AddDbContext<DisputeContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DisputeDbConnection")));

//setup hangfire
builder.Services.AddHangfire(configuration =>
            configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                         .UseSimpleAssemblyNameTypeSerializer()
                         .UseRecommendedSerializerSettings()
                         .UseSqlServerStorage(builder.Configuration.GetConnectionString("DisputeHangfireConnection"), new SqlServerStorageOptions
                         {
                             CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                             SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                             QueuePollInterval = TimeSpan.Zero,
                             UseRecommendedIsolationLevel = true,
                             DisableGlobalLocks = true
                         }));   

builder.Services.AddHangfireServer();

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
app.UseHangfireDashboard("/my-hangfire");

// Configure a recurring job
RecurringJob.AddOrUpdate<DisputeRecurringJob>("createDispute", x => x.CreateDisputeJob(), "*/5 * * * *");



app.Run();


