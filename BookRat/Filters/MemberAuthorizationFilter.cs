using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookRat.Filters
{
    public class MemberAuthorizationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            int? membreId = context.HttpContext.Session.GetInt32("MembreId");

            if (membreId == null)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No implementation needed
        }
    }
}
