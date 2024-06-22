using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;


namespace CRUDExample.Filters.ActionsFilter
{
	public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
	{
		private readonly ICountriesService _countriesService;
		private readonly ILogger _logger;	
		public PersonCreateAndEditPostActionFilter(ICountriesService countriesService, ILogger<PersonCreateAndEditPostActionFilter> logger)
		{
			_countriesService = countriesService;
			_logger = logger;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (context.Controller is PersonsController personsController)
			{
				if (!personsController.ModelState.IsValid)
				{
					List<CountryResponse> countries = await _countriesService.GetAllCountries();
					personsController.ViewBag.Countries = countries.Select(temp =>
					new SelectListItem()
					{
						Text = temp.CountryName,
						Value = temp.CountryID.ToString()
					}
				);
					personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
					var personRequest = context.ActionArguments["personRequest"];
					context.Result = personsController.View(personRequest);
				}
				else await next();
			}
			else await next();
			_logger.LogInformation("After logic");
		}
	}
}
