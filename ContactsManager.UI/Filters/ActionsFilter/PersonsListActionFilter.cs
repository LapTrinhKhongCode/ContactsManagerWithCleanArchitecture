using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionsFilter
{
	public class PersonsListActionFilter : IAsyncActionFilter
	{
		private readonly ILogger<PersonsListActionFilter> _logger;	
		public PersonsListActionFilter(ILogger<PersonsListActionFilter> logger)
		{
			_logger = logger;
		}
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			_logger.LogInformation("{FiltersName}.{MethodName} method - before",
				nameof(PersonsListActionFilter), nameof(OnActionExecutionAsync));
			context.HttpContext.Items["arguments"] = context.ActionArguments;
			await next();
			_logger.LogInformation("{FiltersName}.{MethodName} method - after",
				nameof(PersonsListActionFilter), nameof(OnActionExecutionAsync));
			PersonsController personsController = (PersonsController)context.Controller;
			IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)
				context.HttpContext.Items["arguments"];
			if (parameters != null)
			{
				if (parameters.ContainsKey("searchBy"))
				{
					personsController.ViewData["CurrentSearchBy"] = Convert.ToString(parameters["searchBy"]);
				}
				if (parameters.ContainsKey("searchString"))
				{
					personsController.ViewData["CurrentSearchString"] = Convert.ToString(parameters["searchString"]);
				}
				if (parameters.ContainsKey("sortBy"))
				{
					personsController.ViewData["CurrentSortBy"] = Convert.ToString(parameters["sortBy"]);
				}
				if (parameters.ContainsKey("sortOrder"))
				{
					personsController.ViewData["CurrentSortOrder"] = Convert.ToString(parameters["sortOrder"]);
				}
				personsController.ViewBag.SearchFields = new Dictionary<string, string>()
				{
					{ nameof(PersonResponse.PersonName), "Person Name" },
					{ nameof(PersonResponse.Email), "Email" },
					{ nameof(PersonResponse.DateOfBirth), "Date of Birth" },
					{ nameof(PersonResponse.Gender), "Gender" },
					{ nameof(PersonResponse.CountryID), "Country" },
					{ nameof(PersonResponse.Address), "Address" }
				};
			}
		}
	}
}
