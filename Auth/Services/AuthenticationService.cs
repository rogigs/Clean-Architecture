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
    public interface IAuthenticationService
    {
        Task<(AuthenticationException?, Authentication?)> PostAsync(AuthenticationDTO authenticationDTO);
        Task<(AuthenticationException?, Authentication?)> DeleteAsync(string email);
        Task<(AuthenticationException?, Authentication?)> UpdateAsync(AuthenticationUpdateDTO authenticationUpdateDTO);
        Task<(AuthenticationException?, AuthTokens?)> LoginAsync(AuthenticationDTO authenticationDTO);
        // Task<AuthenticationException?> LogoutAsync(string RefreshToken);
        // Task<AuthenticationException?> RefreshTokenAsync(RefreshTokenDTO refreshTokenDTO);
    }

    public class AuthenticationService(IAuthenticationRepository authenticationRepository, IConfiguration configuration) : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        private readonly IConfiguration _configuration = configuration;

        private static string GenerateRefreshToken() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        private string GenerateJwtToken(Authentication authentication)
        {
            var signingKey = _configuration["JwtSettings:SigningKey"];

            if (string.IsNullOrEmpty(signingKey)) throw new InvalidOperationException("Signing key is not configured in the settings.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //TODO: create ID, it mustn trasnfer sensible data as Email
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

        public async Task<(AuthenticationException?, AuthTokens?)> LoginAsync(AuthenticationDTO authenticationDTO)
        {
            try
            {
                Authentication authentication = new()
                {
                    Email = authenticationDTO.Email,
                    Password = authenticationDTO.Password,
                };

                var auth = await _authenticationRepository.GetByAuth(authentication);

                if (auth == null) return (new AuthenticationException("Invalid email or password."), null);

                var token = GenerateJwtToken(auth);
                var refreshToken = GenerateRefreshToken();

                AuthTokens authTokens = new(token, refreshToken);

                return (null, authTokens);
            }
            catch (Exception ex)
            {
                return (new AuthenticationException("An error occurred while logging.", ex), null);
            }
        }

        public async Task<(AuthenticationException?, Authentication?)> PostAsync(AuthenticationDTO authenticationDTO)
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
                return (new AuthenticationException("An error occurred while creating a authentication.", ex), null);
            }

        }

        public async Task<(AuthenticationException?, Authentication?)> DeleteAsync(string email)
        {
            try
            {
                return (null, await _authenticationRepository.Delete(email));
            }
            catch (Exception ex)
            {
                return (new AuthenticationException("An error occurred while deleting a authentication.", ex), null);
            }
        }

        public async Task<(AuthenticationException?, Authentication?)> UpdateAsync(AuthenticationUpdateDTO authenticationUpdateDTO)
        {
            try
            {
                return (null, await _authenticationRepository.Update(authenticationUpdateDTO));
            }
            catch (Exception ex)
            {
                return (new AuthenticationException("An error occurred while uploading a authentication.", ex), null);
            }
        }
    }
}
