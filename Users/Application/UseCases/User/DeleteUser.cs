using System.Text.Json;
using System.Text;
using Users.Application.Exceptions;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.Application.UseCases
{
    internal sealed class DeleteUser(IUserRepository userRepository, HttpClient httpClient) : IDeleteUser
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<(UserException?, User? )> ExecuteAsync(Guid userId)
        {
            try
            {
                User? user = await _userRepository.Delete(userId);

                if (user == null) return (null, null);

                string json = JsonSerializer.Serialize(new { user.Email });
                StringContent content = new(json, Encoding.UTF8, "application/json");
                HttpRequestMessage requestMessage = new(HttpMethod.Delete, "http://localhost:5127/Api/Authentication")
                {
                    Content = content
                };

                await _httpClient.SendAsync(requestMessage);

                return (null, user);
            }
            catch (Exception ex) {
                return (new UserException("An error occurred while deleting a user.", ex), null);
            }
        }
    }
}
