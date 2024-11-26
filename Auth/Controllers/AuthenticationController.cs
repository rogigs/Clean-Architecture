using Microsoft.AspNetCore.Mvc;
using Auth.Services;

namespace Auth.Controllers
{

    public record AuthenticationDTO(string Email, string Password);
    public record AuthenticationUpdateDTO(string Email, string? Password);

    public interface IAuthenticationController
    {
        Task<IActionResult> PostAsync(AuthenticationDTO authenticationDTO);
        Task<IActionResult> DeleteAsync(string email);
        Task<IActionResult> UpdateAsync(AuthenticationUpdateDTO authenticationUpdateDTO);
    }

    [ApiController]
    [Route("Api/[controller]")]
    public class AuthenticationController(
        IAuthenticationService authenticationService
        ) : ControllerBase, IAuthenticationController
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;


        [HttpPost(Name = "PostAuthentication")]
        public async Task<IActionResult> PostAsync([FromBody] AuthenticationDTO authenticationDTO)
        {
            var (error, authentication) = await _authenticationService.PostAsync(authenticationDTO);

            return error == null ? CreatedAtAction(null, authentication) : BadRequest(new { error.Message });
        }


        [HttpDelete(Name = "DeleteAuthentication")]
        public async Task<IActionResult> DeleteAsync([FromBody] string email)
        {
            var (error, authentication) = await _authenticationService.DeleteAsync(email);

            if (error != null) return BadRequest(new { error.Message });

            return authentication == null
                ? NotFound(new { Message = "Authentication not found" })
                : Ok(authentication);
        }

        [HttpPatch(Name = "UpdateAuthentication")]
        public async Task<IActionResult> UpdateAsync([FromBody] AuthenticationUpdateDTO authenticationUpdateDTO)
        {
            var (error, authentication) = await _authenticationService.UpdateAsync(authenticationUpdateDTO);

            if (error != null) return BadRequest(new { error.Message });

            return authentication == null
                ? NotFound(new { Message = "Authentication not found" })
                : Ok(authentication);
        }
    }


}
