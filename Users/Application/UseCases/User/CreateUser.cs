using Users.Application.Exceptions;
using Users.Application.UseCases.DTO;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.Application.UseCases
{
    internal sealed class CreateUser(IUserRepository userRepository) : ICreateUser
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<(UserException?, User?)> ExecuteAsync(UserDTO userDTO)
        {
            try
            {
                User user = new()
                {
                    Name = userDTO.Name,
                    Email = userDTO.Email, 
                };

                await _userRepository.Add(user);
                return (null, user); 
            }
            catch (Exception ex)
            {
                return (new UserException("An error occurred while creating a user.", ex), null);
            }
        }
    }
}
