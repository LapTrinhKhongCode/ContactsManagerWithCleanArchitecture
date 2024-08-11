using CRUDExample.Filters;
using CRUDExample.Filters.ActionsFilter;
using CRUDExample.Filters.AuthorizationFilter;
using CRUDExample.Filters.ExceptionFilter;
using CRUDExample.Filters.ResourcesFilter;
using CRUDExample.Filters.ResultFilter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
	[Route("[controller]")]
	[TypeFilter(typeof(ResponseHeaderActionFilter),Arguments = new object[]
	{
		"KeyController", "ValueController", 3
	}, Order = 3)]
	[TypeFilter(typeof(HandleExceptionFilter))]
	[TypeFilter(typeof(PersonAlwaysRunResultFilter))]

	public class PersonsController : Controller
	{
		//private fields
		private readonly IPersonsService _personsService;
		private readonly ICountriesService _countriesService;

		//constructor
		public PersonsController(IPersonsService personsService, ICountriesService countriesService)
		{
			_personsService = personsService;
			_countriesService = countriesService;
		}

		//Url: index
		[Route("[action]")]
		[Route("/")]
		[TypeFilter(typeof(PersonsListActionFilter), Order = 4)]
		[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[]
		{
			"My-Key-FromAction", "My-Value-FromAction", 1
		}, Order = 1)]
		[TypeFilter(typeof(PersonListResultFilter))]
		[SkipFilter]
		public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
		{
			//Search
			List<PersonResponse> persons = await _personsService.GetFilteredPersons(searchBy, searchString);

			//Sort
			List<PersonResponse> sortedPersons = await _personsService.GetSortedPersons(persons, sortBy, sortOrder);

			//Chuyển bớt sang Filter, Controller chỉ nên dùng để điều hướng 
			return View(sortedPersons); //Views/Persons/Index.cshtml
		}


		//Executes when the user clicks on "Create Person" hyperlink (while opening the create view)
		//Url: persons/create
		[Route("[action]")]
		[HttpGet]
		[TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "my-key", "my-value", 4 })]
		public async Task<IActionResult> Create()
		{
			List<CountryResponse> countries = await _countriesService.GetAllCountries();
			ViewBag.Countries = countries.Select(temp => new SelectListItem()
			{
				Text = temp.CountryName,
				Value = temp.CountryID.ToString()
			});

			return View();
		}

		[HttpPost]
		//Url: persons/create
		[Route("[action]")]
		[TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
		[TypeFilter(typeof(FeatureDisabledResourceFilter), Arguments = new object[] { false })] // true là tắt nút create persons

		public async Task<IActionResult> Create(PersonAddRequest personRequest)
		{
			//call the service method
			PersonResponse personResponse = await _personsService.AddPerson(personRequest);

			//navigate to Index() action method (it makes another get request to "persons/index"
			return RedirectToAction("Index", "Persons");
		}

		[HttpGet]
		[Route("[action]/{personID}")]
		[TypeFilter(typeof(TokenResultFilter))]
		public async Task<IActionResult> Edit(Guid personID)
		{


			PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personID);
			if(personResponse == null)
			{
				RedirectToAction("View");
			}

			PersonUpdateRequest personRequest = personResponse.ToPersonUpdateRequest();

			List<CountryResponse> countries = await _countriesService.GetAllCountries();
			ViewBag.Countries = countries.Select(temp => 
				new SelectListItem()
				{
					Text = temp.CountryName,Value = temp.CountryID.ToString()

				}
			);
			return View(personRequest);

		}

		[HttpPost]
		[Route("[action]/{personID}")]
		[TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
		[TypeFilter(typeof(TokenAuthorizationFilter))]
		
		public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
		{
			PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);

			if(personResponse == null) {
				return RedirectToAction("Index");
			}
			if(ModelState.IsValid)
			{
				PersonResponse updatedPerson = await _personsService.UpdatePerson(personUpdateRequest);
				return RedirectToAction("Index");
			}else
			{
				List<CountryResponse> countries = await _countriesService.GetAllCountries();
				ViewBag.Countries = countries.Select(temp =>
				new SelectListItem()
				{
					Text = temp.CountryName,
					Value = temp.CountryID.ToString()
				});
				ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
				return View();
			}
		}
		[HttpGet]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Delete(Guid personID)
		{
			 PersonResponse? personResponse = await _personsService.GetPersonByPersonID(personID);

			if(personResponse == null) {
				return RedirectToAction("Index");
			}

			return View(personResponse);
		}
		
		[HttpPost]
		[Route("[action]/{personID}")]
		public async Task<IActionResult> Delete(PersonResponse personResponse)
		{
			if (personResponse == null) {  
				return RedirectToAction("Index"); 
			}
			await _personsService.DeletePerson(personResponse.PersonID);
			return View();
		}

		[Route("PersonsPDF")]
		public async Task<IActionResult> PersonsPDF()
		{
			List<PersonResponse> persons = await _personsService.GetAllPersons();
			return new ViewAsPdf("PersonsPDF", persons, ViewData)
			{
				PageMargins = new Rotativa.AspNetCore.Options.Margins()
				{
					Left = 20,
					Top = 20,
					Right = 20,
					Bottom = 20
				},
				PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
			};
		}


		[Route("PersonsExcel")]
		public async Task<IActionResult> PersonsExcel()
		{
			MemoryStream memoryStream = await _personsService.GetPersonsExcel();
			return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
		}
	}
}
