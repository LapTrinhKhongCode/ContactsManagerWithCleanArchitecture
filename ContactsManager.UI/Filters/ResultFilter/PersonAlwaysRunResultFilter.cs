using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilter
{
	public class PersonAlwaysRunResultFilter : IAlwaysRunResultFilter
	{
		public void OnResultExecuted(ResultExecutedContext context)
		{

		}

		public void OnResultExecuting(ResultExecutingContext context)
		{
			if(context.Filters.OfType<SkipFilter>().Any()) {
				return;
			}
		}
	}
}
