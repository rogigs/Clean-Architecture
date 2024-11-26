using Users.Application.Exceptions;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.Application.UseCases
{
    internal sealed class DeleteUser(IUserRepository userRepository) : IDeleteUser
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<(UserException?, User? )> ExecuteAsync(Guid userId)
        {
            try
            {
                return (null, await _userRepository.Delete(userId));
            }
            catch (Exception ex) {
                return (new UserException("An error occurred while deleting a user.", ex), null);
            }
        }
    }
}
