using ContactsManager.Core.Domain.IdentityEntities;
using CRUDExample.Filters.ActionsFilter;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace ContactsManager.UI.StartupExtensions
{
	public static class ConfigureServicesExtension
	{
		public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddControllersWithViews(options =>
			{
				var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
				options.Filters.Add(new ResponseHeaderActionFilter(logger, "KeyGlobal", "ValueGlobal", 2));
			});

			//add services into IoC container

			services.AddScoped<ICountriesRepository, CountriesRepository>();
			services.AddScoped<IPersonsRepository, PersonsRepository>();

			services.AddScoped<ServiceContracts.ICountriesService, CountriesService>();
			services.AddScoped<IPersonsService, PersonsService>();



			//thông báo add db sử dụng với sqlserver
			services.AddDbContext<Entities.ApplicationDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("DefautConnection"));
			});
			//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

			//thêm identity
			services.AddIdentity<ApplicationUser,ApplicationRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders()
				.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext,Guid>>()
				.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

			return services;
		}

	}
}
