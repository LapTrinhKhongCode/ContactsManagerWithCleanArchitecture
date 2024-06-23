using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Serilog;
using CRUDExample.Filters.ActionsFilter;
using RepositoryContracts;
using Repositories;

var services = WebApplication.CreateBuilder(args);

//builder.Host.ConfigureLogging(loggingProvider =>
//{
//	loggingProvider.ClearProviders();	
//	loggingProvider.AddConsole();
//	loggingProvider.AddDebug();
//	loggingProvider.AddEventLog();
//});

services.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
	loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

var app = services.Build();

//app.Logger.LogDebug("LogDebug");
//app.Logger.LogInformation("LogInformation");
//app.Logger.LogWarning("LogWarning");
//app.Logger.LogError("LogError");
//app.Logger.LogCritical("LogCritical");

if (services.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot",wkhtmltopdfRelativePath: "Rotativa");	
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
