using Microsoft.AspNetCore.Mvc;
using Auth.Services;
using Microsoft.AspNetCore.Authorization;

namespace Auth.Controllers
{

    public record AuthenticationDTO(string Email, string Password);
    public record AuthenticationUpdateDTO(string Email, string? Password, string? NewPassword);
    public record RefreshTokenDTO(string Token, string RefreshToken);


    public interface IAuthenticationController
    {
        Task<IActionResult> PostAsync(AuthenticationDTO authenticationDTO);
        Task<IActionResult> DeleteAsync(string email);
        Task<IActionResult> UpdateAsync(AuthenticationUpdateDTO authenticationUpdateDTO);
        Task<IActionResult> LoginAsync(AuthenticationDTO authenticationDTO);
        //Task<IActionResult> LogoutAsync(string RefreshToken);
        //Task<IActionResult> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO);
    }

    [ApiController]
    [Route("Api/[controller]")]
    public class AuthenticationController(
        IAuthenticationService authenticationService
        ) : ControllerBase, IAuthenticationController
    {
        private readonly IAuthenticationService _authenticationService = authenticationService;

        [HttpPost(Name = "AuthenticationUser")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthenticationDTO authenticationDTO)
        {
            var (error, authentication) = await _authenticationService.LoginAsync(authenticationDTO);

            return authentication == null
                ? Unauthorized(new { error!.Message }) 
                : Ok(new { authentication.Token, authentication.RefreshToken });
        }

        //[HttpPost("logout", Name = "DeauthenticationUser")]
        //public async Task<IActionResult> LogoutAsync([FromBody] string refreshToken)
        //{
        //    var result = await _authenticationService.DeauthenticationAsync(refreshToken);

        //    return result.Success
        //        ? Ok(new { Message = "Logout successful" })
        //        : BadRequest(new { result.ErrorMessage });
        //}

        //[HttpPost(Name = "RefreshToken")]
        //public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDTO refreshTokenDTO)
        //{
        //    var result = await _authenticationService.RefreshTokenAsync(refreshTokenDTO);

        //    return result.Success
        //        ? Ok(new { Token = result.Token, RefreshToken = result.NewRefreshToken })
        //        : Unauthorized(new { result.ErrorMessage });
        //}


        [HttpPost("CreateUser", Name = "PostAuthentication")]
        public async Task<IActionResult> PostAsync([FromBody] AuthenticationDTO authenticationDTO)
        {
            var (error, authentication) = await _authenticationService.PostAsync(authenticationDTO);

            return error == null ? CreatedAtAction(null, authentication!.Email): BadRequest(new { error.Message });
        }


        [Authorize]
        [HttpDelete(Name = "DeleteAuthentication")]
        public async Task<IActionResult> DeleteAsync([FromBody] string email)
        {
            var (error, authentication) = await _authenticationService.DeleteAsync(email);

            if (error != null) return BadRequest(new { error.Message });

            return authentication == null
                ? NotFound(new { Message = "Authentication not found" })
                : Ok(authentication.Email);
        }

        [Authorize]
        [HttpPatch(Name = "UpdateAuthentication")]
        public async Task<IActionResult> UpdateAsync([FromBody] AuthenticationUpdateDTO authenticationUpdateDTO)
        {
            var (error, authentication) = await _authenticationService.UpdateAsync(authenticationUpdateDTO);

            if (error != null) return BadRequest(new { error.Message });

            return authentication == null
                ? NotFound(new { Message = "Authentication not found" })
                : Ok(authentication.Email);
        }
    }


}
