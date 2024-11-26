using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clean_Architecture.Application.Validations
{
    public class ValidatePaginationAttributes : ActionFilterAttribute
    {
        private static readonly int[] AllowedTakeValues = [10, 25, 50];

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (HasPaginationArguments(context, out int take, out int skip))
            {
                if (!IsValidTake(take))
                {
                    context.Result = new BadRequestObjectResult("Take must be 10, 25, or 50.");
                    return;
                }

                if (!IsValidSkip(skip))
                {
                    context.Result = new BadRequestObjectResult("Skip must be a non-negative integer.");
                    return;
                }
            }

            base.OnActionExecuting(context);
        }

        private static bool HasPaginationArguments(ActionExecutingContext context, out int take, out int skip)
        {
            take = 0;
            skip = 0;

            var hasTake = context.ActionArguments.TryGetValue("take", out var takeObj) && takeObj is int t && (take = t) >= 0;
            var hasSkip = context.ActionArguments.TryGetValue("skip", out var skipObj) && skipObj is int s && (skip = s) >= 0;

            return hasTake && hasSkip;
        }

        private static bool IsValidTake(int take)
        {
            return AllowedTakeValues.Contains(take);
        }

        private static bool IsValidSkip(int skip)
        {
            return skip >= 0;
        }

    }
}
