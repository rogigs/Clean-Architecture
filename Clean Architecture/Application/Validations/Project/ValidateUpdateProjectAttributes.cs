using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Clean_Architecture.Application.UseCases.DTO;

namespace Clean_Architecture.Application.Validations
{
    public class ValidateUpdateProjectAttributes : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!TryGetProjectUpdateDto(context, out var projectDTO))
            {
                context.Result = new BadRequestObjectResult("Invalid parameters.");
                return;
            }

            if (AreAllParametersNullOrEmpty(projectDTO?.Name, projectDTO?.Description, projectDTO?.EndDate))
            {
                context.Result = new BadRequestObjectResult("At least one of Name, Description, or EndDate must be provided.");
                return;
            }

            base.OnActionExecuting(context);
        }
        private static bool TryGetProjectUpdateDto(ActionExecutingContext context, out ProjectUpdateDTO? projectDTO)
        {
            bool hasProjectDto = context.ActionArguments.TryGetValue("projectDTO", out var projectDtoObject);
            bool isProjectUpdateDto = projectDtoObject is ProjectUpdateDTO;
            bool isValidProjectDto = hasProjectDto && isProjectUpdateDto;

            projectDTO = (ProjectUpdateDTO?)(isValidProjectDto ? projectDtoObject : null);

            return projectDTO != null;
        }

        private static bool AreAllParametersNullOrEmpty(string? name, string? description, DateTime? endDate)
        {
            return string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(description) && !endDate.HasValue;
        }
    }
}
