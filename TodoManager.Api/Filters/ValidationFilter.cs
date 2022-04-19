using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TodoManager.Api.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // before controller action
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                kv => kv.Value.Errors.Select(e => e.ErrorMessage));

            context.Result = new BadRequestObjectResult(errors);

            return;
        }

        await next();

        // after controller action
    }
}
