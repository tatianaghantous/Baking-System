using System.Reflection;
using Foxera.Common.Settings;
using Foxera.HealthCheck;
using Foxera.Keycloak;
using Foxera.Logging;
using Foxera.Logging.Middlewares;
using Foxera.Mail;
using Foxera.RabitMq;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.Extensions.Options;
using UAS.API;
using UAS.API.RabbitMQ;
using UAS.API.Startup;
using UAS.Application;
using UAS.Persistence;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Services.AddControllers();
services.AddPersistenceServices(configuration);
services.AddApplicationServices(configuration);
services.AddWebApiServices(builder.Configuration);

//services.AddBackgroundJobsSettings(configuration);
//services.AddBackgroundJobServices();
services.AddMailServices(configuration);
//services.AddLocalizationSettings(configuration);
//services.AddLocalizationServices();
builder.AddLoggingServices();
// Configure RabbitMQ settings
services.Configure<RabitMQSettings>(configuration.GetSection(nameof(RabitMQSettings)));

// Register the RabbitMQ service
// Register the RabbitMQ service
services.AddSingleton<IRabbitMQService, RabbitMQService>(sp =>
{
    var rabbitMQSettings = sp.GetRequiredService<IOptions<RabitMQSettings>>().Value;
    return new RabbitMQService(rabbitMQSettings.Host);
});
services.AddHostedService<HostedServices>();
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