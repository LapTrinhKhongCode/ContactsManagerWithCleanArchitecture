using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;
using Serilog;
using CRUDExample.Filters.ActionsFilter;
using RepositoryContracts;
using Repositories;

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

builder.Services.AddControllersWithViews(options =>
{
	var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
	options.Filters.Add(new ResponseHeaderActionFilter(logger, "KeyGlobal","ValueGlobal",2));
});

//add services into IoC container

builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>()	;

builder.Services.AddScoped<ServiceContracts.ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();



//thông báo add db sử dụng với sqlserver
builder.Services.AddDbContext<Entities.ApplicationDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefautConnection"));
});
//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False
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
app.MapControllers();

app.Run();
