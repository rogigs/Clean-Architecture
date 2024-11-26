using Auth.Controllers;
using Auth.Database.Entities;
using Auth.Database.Repositories;
using Auth.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Services
{
    public interface IAuthenticationService
    {
        Task<(AutheticationException?, Authentication?)> PostAsync(AuthenticationDTO authenticationDTO);
        Task<(AutheticationException?, Authentication?)> DeleteAsync(string email);
        Task<(AutheticationException?, Authentication?)> UpdateAsync(AuthenticationUpdateDTO authenticationUpdateDTO);
    }

    public class AuthenticationService(IAuthenticationRepository authenticationRepository): IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;

        public async Task<(AutheticationException?, Authentication?)> PostAsync(AuthenticationDTO authenticationDTO)
        {
            try
            {
                Authentication authentication = new()
                {
                    Email = authenticationDTO.Email,
                    Password = authenticationDTO.Password,
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
