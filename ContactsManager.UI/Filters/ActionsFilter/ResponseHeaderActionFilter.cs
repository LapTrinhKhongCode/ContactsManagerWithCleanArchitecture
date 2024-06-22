using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionsFilter
{
	public class ResponseHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
	{
		private readonly ILogger<ResponseHeaderActionFilter> _logger;
		private string Key;
		private string Value;	
		public int Order { get; set; }
		public ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger, string key, string value, int order)
		{
			_logger = logger;
			Key = key;
			Value = value;
			Order = order;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			_logger.LogInformation("{FiltersName}.{MethodName} method - before",
				nameof(PersonsListActionFilter), nameof(OnActionExecutionAsync));
			await next();
			_logger.LogInformation("{FiltersName}.{MethodName} method - after",
				nameof(PersonsListActionFilter), nameof(OnActionExecutionAsync));
		}
	}
}
