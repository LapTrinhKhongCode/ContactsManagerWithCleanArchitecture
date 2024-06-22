using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ExceptionFilter
{
	public class HandleExceptionFilter : IExceptionFilter
	{
		private readonly ILogger<HandleExceptionFilter> _logger;	
		private readonly IHostEnvironment _hostEnvironment;	
		public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment hostEnvironment)
		{
			_logger = logger;
			_hostEnvironment = hostEnvironment;	
		}
		public void OnException(ExceptionContext context)
		{
			_logger.LogError("Exception filter {FilterName}.{MethodName}\n{ExceptionType}{ExceptionMessage}",
				nameof(HandleExceptionFilter),nameof(OnException),context.Exception.GetType().ToString(),
				context.Exception.Message);
			context.Result = new ContentResult()
			{
				Content =
				context.Exception.Message, StatusCode = 500
			};
		}
	}
}
