using System.Text.Json;
using System.Text;
using Users.Application.Exceptions;
using Users.Application.UseCases.DTO;
using Users.Domain.Entities;
using Users.Domain.Interfaces;
using Users.Infrastructure;

namespace Users.Application.UseCases
{
    internal sealed class CreateUser(IUserRepository userRepository, HttpClient httpClient) : ICreateUser
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly HttpClient _httpClient = httpClient;

        public async Task<(UserException?, User?)> ExecuteAsync(UserDTO userDTO)
        {
            try
            {
                // TODO: Implement consistency with data between two services
                string json = JsonSerializer.Serialize(new { userDTO.Email, userDTO.Password });
                StringContent content = new(json, Encoding.UTF8, "application/json");

                await _httpClient.PostAsync("http://localhost:5127/Api/Authentication/CreateUser", content);

                User user = new()
                {
                    Name = userDTO.Name,
                    Email = userDTO.Email,
                };

                var messageObj = new
                {
                    UserId = 123,
                    UserName = "John Doe",
                    Email = "john.doe@example.com"
                };

                RabbitMQConnection rabbitMqConnection = new();
                await rabbitMqConnection.InitializeAsync();
                await rabbitMqConnection.SendMessageAsync("sendUserIdToProject", JsonSerializer.Serialize(new { user.UserId, userDTO.ProjectId }));
                await rabbitMqConnection.CloseAsync();

                await _userRepository.Add(user);
                return (null, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (new UserException("An error occurred while creating a user.", ex), null);
            }
        }
    }
}
