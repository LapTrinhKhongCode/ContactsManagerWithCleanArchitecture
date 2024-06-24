using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Serilog;
using CRUDExample.Filters.ActionsFilter;
using RepositoryContracts;
using Repositories;
using ContactsManager.UI.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.ConfigureLogging(loggingProvider =>
//{
//	loggingProvider.ClearProviders();	
//	loggingProvider.AddConsole();
//	loggingProvider.AddDebug();
//	loggingProvider.AddEventLog();
//});

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
	loggerConfiguration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services);
});

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

//app.Logger.LogDebug("LogDebug");
//app.Logger.LogInformation("LogInformation");
//app.Logger.LogWarning("LogWarning");
//app.Logger.LogError("LogError");
//app.Logger.LogCritical("LogCritical");

if (builder.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot",wkhtmltopdfRelativePath: "Rotativa");	

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
