using System.Reflection;
using Foxera.BackgroundJobs;
using Foxera.Caching;
using Foxera.Common.Settings;
using Foxera.Common.Startup;
using Foxera.HealthCheck;
using Foxera.Keycloak;
using Foxera.Localization;
using Foxera.Logging;
using Foxera.Logging.Middlewares;
using Foxera.Mail;
using Foxera.RabitMq;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Options;
using TS.API;
using TS.API.RabbitMQ;
using TS.API.Startup;
using TS.Persistence;
using TS.Persistence.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers();
services.Configure<RabitMQSettings>(configuration.GetSection(nameof(RabitMQSettings)));
services.AddSingleton<IRabbitMQService, RabbitMQService>();
services.AddWebApiServices(builder.Configuration);
services.AddHostedService<HostedServices>();
services.AddPersistenceServices(configuration);
services.AddApplicationServices(configuration);


services.AddBackgroundJobsSettings(configuration);
services.AddBackgroundJobServices();
services.AddMailServices(configuration);
// services.AddLocalizationSettings(configuration);
// services.AddLocalizationServices();
builder.AddLoggingServices();
// Configure RabbitMQ settings


// Register the RabbitMQ service
// Register the RabbitMQ service

builder.Services.AddAuthenticationServices(configuration);
//services.AddCachingServices(configuration);
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(Program)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
services.AddSwaggerServices();
//services.AddHealthCheckServices(configuration);//should be bel ekhir ,lieanno eza ha nestaamil 
services.AddTransient<IStartupFilter, SettingValidationStartupFilter>();//kermel naamil the validation for all settings

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ExecuteDbMigrations();
app.UseBackgroundJobServices();

RecurringJob.AddOrUpdate<RecurrentTransactionProcessor>(
    "process-recurrent-transactions",
    x => x.ProcessRecurrentTransaction(),
    Cron.Daily 
);
//app.UseMiddleware<StartupTasksMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();
//app.UseBackgroundJobServices();
//app.UseLocalizationServices();
//app.UseHealthChecksService();
app.UseLoggingServices();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();