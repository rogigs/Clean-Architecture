using System.Security.Claims;
using System.Text;
using Auth.Controllers;
using Auth.Database.Entities;
using Auth.Database.Repositories;
using Auth.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using BCrypt.Net;


namespace Auth.Services
{
    public record AuthTokens(
      string Token,
      string RefreshToken
    );

    //TODO: add Refresh Token and Logout
    // Task<AuthenticationException?> LogoutAsync(string RefreshToken);
    // Task<AuthenticationException?> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO);

    public interface IAuthenticationService
    {
        Task<(AutheticationException?, Authentication?)> PostAsync(AuthenticationDTO authenticationDTO);
        Task<(AutheticationException?, Authentication?)> DeleteAsync(string email);
        Task<(AutheticationException?, Authentication?)> UpdateAsync(AuthenticationUpdateDTO authenticationUpdateDTO);
        Task<(AutheticationException?, AuthTokens?)> LoginAsync(AuthenticationDTO authenticationDTO);
    }

    public class AuthenticationService(IAuthenticationRepository authenticationRepository, IConfiguration configuration) : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        private readonly IConfiguration _configuration = configuration;

        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        private string GenerateJwtToken(Authentication authentication)
        {

            var signingKey = _configuration["JwtSettings:SigningKey"];

            if (string.IsNullOrEmpty(signingKey)) throw new InvalidOperationException("Signing key is not configured in the settings.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            Claim[] claims =
            [
                 new Claim(JwtRegisteredClaimNames.Sub, authentication.Email),
            ];

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<(AutheticationException?, AuthTokens?)> LoginAsync(AuthenticationDTO authenticationDTO)
        {
            try
            {
                Authentication authentication = new()
                {
                    Email = authenticationDTO.Email,
                    Password = authenticationDTO.Password,
                };

                var auth = await _authenticationRepository.GetByAuth(authentication);

                if (auth == null) return (new AutheticationException("Invalid email or password."), null);

                var token = GenerateJwtToken(auth);
                var refreshToken = GenerateRefreshToken();

                AuthTokens authTokens = new(token, refreshToken);

                return (null, authTokens);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (new AutheticationException("An error occurred while logging.", ex), null);
            }
        }

        public async Task<(AutheticationException?, Authentication?)> PostAsync(AuthenticationDTO authenticationDTO)
        {
            try
            {
                Authentication authentication = new()
                {
                    Email = authenticationDTO.Email,
                    Password = authenticationDTO.Password
                };

                await _authenticationRepository.Add(authentication);
                return (null, authentication);
            }
            catch (Exception ex)
            {
                return (new AutheticationException("An error occurred while creating a authentication.", ex), null);
            }

        }

        public async Task<(AutheticationException?, Authentication?)> DeleteAsync(string email)
        {
            try
            {
                return (null, await _authenticationRepository.Delete(email));
            }
            catch (Exception ex)
            {
                return (new AutheticationException("An error occurred while deleting a authentication.", ex), null);
            }
        }

        public async Task<(AutheticationException?, Authentication?)> UpdateAsync(AuthenticationUpdateDTO authenticationUpdateDTO)
        {
            try
            {
                return (null, await _authenticationRepository.Update(authenticationUpdateDTO));
            }
            catch (Exception ex)
            {
                return (new AutheticationException("An error occurred while uploading a authentication.", ex), null);
            }
        }
    }
}
