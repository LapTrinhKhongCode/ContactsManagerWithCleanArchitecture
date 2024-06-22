using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilter
{
	public class PersonListResultFilter : IAsyncResultFilter
	{
		private readonly ILogger<PersonListResultFilter> _logger;
		public PersonListResultFilter(ILogger<PersonListResultFilter> logger) { 
			_logger = logger;	
		}
		public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			_logger.LogInformation("{FilterName}.{MethodName} - before", nameof(PersonListResultFilter),nameof(OnResultExecutionAsync));
			await next();
			_logger.LogInformation("{FilterName}.{MethodName} - after", nameof(PersonListResultFilter), nameof(OnResultExecutionAsync));

			context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
		}
	}
}
