using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Users.Application.UseCases.DTO;

namespace Users.Application.Validations
{
    public class ValidateUpdateUserAttributes : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!TryGetUserUpdateDTO(context, out var userDTO))
            {
                context.Result = new BadRequestObjectResult("Invalid parameters.");
                return;
            }

            if (AreAllParametersNullOrEmpty(userDTO?.Name, userDTO?.Email))
            {
                context.Result = new BadRequestObjectResult("At least one of Name or Email must be provided.");
                return;
            }

            base.OnActionExecuting(context);
        }
        private static bool TryGetUserUpdateDTO(ActionExecutingContext context, out UserUpdateDTO? userDTO)
        {
            bool hasUserDTO = context.ActionArguments.TryGetValue("userDTO", out var userDTOObject);
            bool isUserUpdateDTO = userDTOObject is UserUpdateDTO;
            bool isValidUserDTO = hasUserDTO && isUserUpdateDTO;

            userDTO = (UserUpdateDTO?)(isValidUserDTO ? userDTOObject : null);

            return userDTO != null;
        }

        private static bool AreAllParametersNullOrEmpty(string? name, string? email)
        {
            return string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(email);
        }
    }
}
