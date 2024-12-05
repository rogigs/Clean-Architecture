using System.Text;
using System.Text.Json;
using Users.Application.Exceptions;
using Users.Application.UseCases.DTO;
using Users.Domain.Entities;
using Users.Domain.Interfaces;
using Users.Infrastructure;

namespace Users.Application.UseCases
{
    internal sealed class CreateUser(
        IUserRepository userRepository,
        HttpClient httpClient,
        IConfiguration configuration
    ) : ICreateUser
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly string _authServiceBaseUrl = configuration["Services:Auth"] ?? throw new ArgumentNullException("Services:Auth", "Auth service base URL is not configured.");

        private readonly HttpClient _httpClient = httpClient;

        public async Task<(UserException?, User?)> ExecuteAsync(UserDTO userDTO)
        {
            try
            {
                string json = JsonSerializer.Serialize(new { userDTO.Email, userDTO.Password });
                StringContent content = new(json, Encoding.UTF8, "application/json");

                HttpResponseMessage authResponse = await _httpClient.PostAsync(
                    _authServiceBaseUrl + "Authentication/CreateUser",
                    content
                );

                if (!authResponse.IsSuccessStatusCode)
                    return (
                        new UserException("Failed to create user in the authentication service."),
                        null
                    );

                User user = new() { Name = userDTO.Name, Email = userDTO.Email };

                // TODO: add manual reversal
                await _userRepository.Add(user);

                // TODO: Add outbox pattern
                RabbitMQConnection rabbitMqConnection = RabbitMQConnection.Instance;
                var (exception, status) = await rabbitMqConnection.Initialization;

                if (exception != null)
                    return (new UserException(exception.Message, exception), null);

                await rabbitMqConnection.SendMessageAsync(
                    "sendUserIdToProject",
                    JsonSerializer.Serialize(new { user.UserId, userDTO.ProjectId })
                );
                await rabbitMqConnection.CloseAsync();

                return (null, user);
            }
            catch (Exception ex)
            {
                return (new UserException("An error occurred while creating a user.", ex), null);
            }
        }
    }
}
