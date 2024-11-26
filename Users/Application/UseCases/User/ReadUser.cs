using Users.Application.Exceptions;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.Application.UseCases
{
    internal sealed class ReadUser(IUserRepository userRepository) : IReadUser
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<(UserException?, User?)> ExecuteAsync(Guid userId)
        {
            try
            {
                return (null, await _userRepository.GetById(userId));
            }
            catch (Exception ex)
            {
                return (new UserException("An error occurred while getting a user.", ex), null);
            }
        }
    }
}
