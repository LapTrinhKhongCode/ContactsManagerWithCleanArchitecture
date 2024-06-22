using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace CRUDExample.Filters.AuthorizationFilter
{
	public class TokenAuthorizationFilter : IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (!context.HttpContext.Request.Cookies.ContainsKey("Auth-Key"))
			{
				context.Result = new StatusCodeResult(404);
				return;
			}
			if (context.HttpContext.Request.Cookies["Auth-Key"] != "A100")
			{
				context.Result = new StatusCodeResult(404);
				return;
			}
		}
	}
}
