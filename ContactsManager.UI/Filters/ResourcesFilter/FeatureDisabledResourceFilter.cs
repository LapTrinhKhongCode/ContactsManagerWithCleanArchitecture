using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResourcesFilter
{
	public class FeatureDisabledResourceFilter : IAsyncResourceFilter
	{
		private readonly ILogger<FeatureDisabledResourceFilter> _logger;
		private readonly bool _disabled;
		public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool disabled = true)
		{
			_logger = logger;
			_disabled = disabled;
		}

		public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
		{
			_logger.LogInformation("{FilterName}.{MethodName} - before", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));
			if (_disabled)
			{
				context.Result = new StatusCodeResult(501);
			}
			else await next();
			_logger.LogInformation("{FilterName}.{MethodName} - after", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));

		}
	}
}
