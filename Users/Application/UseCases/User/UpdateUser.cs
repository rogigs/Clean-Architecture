using System.Text.Json;
using System.Text;
using Users.Application.Exceptions;
using Users.Application.UseCases.DTO;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.Application.UseCases
{
    internal sealed class UpdateUser(IUserRepository userRepository, HttpClient httpClient) : IUpdateUser
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly HttpClient _httpClient = httpClient;

        public Task<(UserException?, User?)> ExecuteAsync(UserUpdateDTO project)
        {
            throw new NotSupportedException("This method is not supported and should not be implemented.");
        }

        public async Task<(UserException?, User?)> ExecuteAsync(UserUpdateDTO userUpdateDTO, Guid userId)
        {
            try
            {
                bool isToSyncAuthService = !string.IsNullOrEmpty(userUpdateDTO?.Email);
                Console.WriteLine(isToSyncAuthService);
                Console.WriteLine(userUpdateDTO);
                if (isToSyncAuthService)
                {
                    string json = JsonSerializer.Serialize(new { userUpdateDTO!.Email, userUpdateDTO?.NewEmail, userUpdateDTO?.Password, userUpdateDTO?.NewPassword });
                    StringContent content = new(json, Encoding.UTF8, "application/json");

                    await _httpClient.PatchAsync("http://localhost:5127/Api/Authentication", content);
                }

                return (null, await _userRepository.Update(userUpdateDTO!, userId));
            }
            catch (Exception ex)
            {
                return (new UserException("An error occurred while uploading a user.", ex), null);
            }
        }
    }
}
