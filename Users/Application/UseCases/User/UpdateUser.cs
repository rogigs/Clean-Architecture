using Users.Application.Exceptions;
using Users.Application.UseCases.DTO;
using Users.Domain.Entities;
using Users.Domain.Interfaces;

namespace Users.Application.UseCases
{
    internal sealed class UpdateUser(IUserRepository userRepository) : IUpdateUser
    {
        private readonly IUserRepository _userRepository = userRepository;

        public Task<(UserException?, User?)> ExecuteAsync(UserUpdateDTO project)
        {
            throw new NotSupportedException("This method is not supported and should not be implemented.");

        }

        public async Task<(UserException?, User?)> ExecuteAsync(UserUpdateDTO userUpdateDTO, Guid userId)
        {
            try
            {
                return (null, await _userRepository.Update(userUpdateDTO, userId));
            }
            catch (Exception ex)
            {
                return (new UserException("An error occurred while uploading a user.", ex), null);
            }
        }
    }
}
